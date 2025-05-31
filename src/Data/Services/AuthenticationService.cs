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

        public async Task<string> RegisterPersonAsync(PersonRegisterRequest request)
        {
            // 1. Перевіряємо дублікати за Email
            var existingUser = await _users
                .Find(u => u.Email == request.Email.Trim().ToLower())
                .FirstOrDefaultAsync();
            if (existingUser != null)
                throw new Exception("Користувач із таким Email уже існує.");

            // 2. Хешуємо пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Формуємо AppUser
            var newUser = new AppUser
            {
                UserName = request.Email.Trim().ToLower(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                IsEmailConfirmed = true,
                Roles = new List<string> { "Person" },

                FullName = request.FullName.Trim(),
                PhoneNumber = request.PhoneNumber.Trim(),
                City = request.City.Trim(),
                Biography = request.Biography.Trim(),
                ContentCategories = request.ContentCategories.Trim(),
                InstagramHandle = request.InstagramHandle.Trim(),
                YoutubeHandle = request.YoutubeHandle?.Trim(),
                TiktokHandle = request.TiktokHandle?.Trim(),
                TelegramHandle = request.TelegramHandle?.Trim()
            };

            // 4. Вставляємо в MongoDB
            await _users.InsertOneAsync(newUser);

            // 5. Перевіряємо / створюємо AppRole
            var normalizedRole = "PERSON";
            var existingRole = await _roles
                .Find(r => r.NormalizedName == normalizedRole)
                .FirstOrDefaultAsync();
            if (existingRole == null)
            {
                var newRole = new AppRole
                {
                    Name = "Person",
                    NormalizedName = normalizedRole
                };
                await _roles.InsertOneAsync(newRole);
            }

            // 6. Повертаємо JWT
            return GenerateJwtToken(newUser);
        }

        public async Task<string> RegisterCompanyAsync(CompanyRegisterRequest request)
        {
            // 1. Перевіряємо дублікати за Email
            var existingUser = await _users
                .Find(u => u.Email == request.Email.Trim().ToLower())
                .FirstOrDefaultAsync();
            if (existingUser != null)
                throw new Exception("Користувач із таким Email уже існує.");

            // 2. Хешуємо пароль
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Формуємо AppUser
            var newUser = new AppUser
            {
                UserName = request.Email.Trim().ToLower(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = passwordHash,
                IsEmailConfirmed = true,
                Roles = new List<string> { "Company" },

                FullName = request.CompanyName.Trim(), // щоб у токені зберігати хоча б назву
                CompanyName = request.CompanyName.Trim(),
                ContactPerson = request.ContactPerson.Trim(),
                CompanyPhone = request.CompanyPhone.Trim(),
                Website = request.Website?.Trim(),
                Industry = request.Industry.Trim(),
                CompanySize = request.CompanySize.Trim(),
                CompanyDescription = request.CompanyDescription.Trim(),
                CollaborationGoals = request.CollaborationGoals.Trim(),
                BudgetRange = request.BudgetRange.Trim(),
                TargetAudience = request.TargetAudience.Trim()
            };

            // 4. Вставка в MongoDB
            await _users.InsertOneAsync(newUser);

            // 5. Перевірка/створення ролі
            var normalizedRole = "COMPANY";
            var existingRole = await _roles
                .Find(r => r.NormalizedName == normalizedRole)
                .FirstOrDefaultAsync();
            if (existingRole == null)
            {
                var newRole = new AppRole
                {
                    Name = "Company",
                    NormalizedName = normalizedRole
                };
                await _roles.InsertOneAsync(newRole);
            }

            // 6. Повертаємо JWT
            return GenerateJwtToken(newUser);
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _users
                .Find(u => u.Email == request.Email.Trim().ToLower())
                .FirstOrDefaultAsync();
            if (user == null)
                throw new Exception("Невірний Email або пароль.");

            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!verified)
                throw new Exception("Невірний Email або пароль.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? string.Empty)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

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
