using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Mongo;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace bestvinnytsa.web.Data.Services
{
    /// <summary>
    /// Реалізація аутентифікації через MongoDB:
    /// - Використовує BCrypt.Net-Next для хешування паролів.
    /// - Зберігає AppUser у колекцію "Users" та AppRole у колекцію "Roles".
    /// - Генерує JWT на основі налаштувань JwtSettings.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMongoCollection<AppUser> _users;
        private readonly IMongoCollection<AppRole> _roles;
        private readonly JwtSettings _jwtSettings;

        public AuthenticationService(
            MongoContext mongoContext,
            IOptions<JwtSettings> jwtOptions)
        {
            _users = mongoContext.Users;
            _roles = mongoContext.Roles;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            // 1) Перевіряємо, чи користувач із таким email уже існує
            var existingUser = await _users
                .Find(u => u.Email == request.Email)
                .FirstOrDefaultAsync();

            if (existingUser != null)
                throw new Exception("Користувач із таким Email уже існує.");

            // 2) Хешуємо пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3) Формуємо новий обʼєкт AppUser
            var newUser = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                PasswordHash = passwordHash,
                IsEmailConfirmed = true,
                Roles = new List<string> { request.Role }
            };

            // 4) Вставляємо користувача в колекцію Users
            await _users.InsertOneAsync(newUser);

            // 5) Перевіряємо, чи існує відповідна AppRole в колекції Roles
            var normalizedRole = request.Role.ToUpperInvariant();
            var existingRole = await _roles
                .Find(r => r.NormalizedName == normalizedRole)
                .FirstOrDefaultAsync();

            if (existingRole == null)
            {
                var newRole = new AppRole
                {
                    Name = request.Role,
                    NormalizedName = normalizedRole
                };
                await _roles.InsertOneAsync(newRole);
            }

            // 6) Генеруємо JWT і повертаємо
            return GenerateJwtToken(newUser);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            // 1) Шукаємо користувача за email
            var user = await _users
                .Find(u => u.Email == request.Email)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("Невірний Email або пароль.");

            // 2) Перевіряємо пароль через BCrypt
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!verified)
                throw new Exception("Невірний Email або пароль.");

            // 3) Генеруємо JWT і повертаємо
            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Створює JWT-токен з Claim:
        /// - ClaimTypes.NameIdentifier = user.Id
        /// - ClaimTypes.Email = user.Email
        /// - ClaimTypes.Role = кожна роль із user.Roles
        /// - Claim "FullName" = user.FullName
        /// </summary>
        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? "")
            };

            // Додаємо ролі
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Формуємо ключ і підписи
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Створюємо і повертаємо JWT
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
