using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;
using bestvinnytsa.web.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace bestvinnytsa.web.Data.Services
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAndGenerateTokenAsync(RegisterRequest request);
        Task<string> LoginAndGenerateTokenAsync(LoginRequest request);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager   = userManager;
            _roleManager   = roleManager;
            _configuration = configuration;
        }

        public async Task<string> RegisterAndGenerateTokenAsync(RegisterRequest request)
        {
        
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                throw new Exception("Користувач із таким Email вже існує.");

   
            var user = new AppUser
            {
                UserName = request.Email,
                Email    = request.Email,
                FullName = request.FullName,
                RoleId   = request.RoleId,
                IsEmailConfirmed = true
            };
            var createResult = await _userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join("; ", createResult.Errors);
                throw new Exception($"Не вдалося створити користувача: {errors}");
            }

          
            string roleName = request.RoleId == 1 ? "Producer" : "Influencer";
          
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
      
            await _userManager.AddToRoleAsync(user, roleName);

      
            return await GenerateJwtTokenAsync(user);
        }

        public async Task<string> LoginAndGenerateTokenAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Невірний Email або пароль.");

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!validPassword)
                throw new Exception("Невірний Email або пароль.");

            return await GenerateJwtTokenAsync(user);
        }

        private async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey   = jwtSettings.GetValue<string>("SecretKey");
            var issuer      = jwtSettings.GetValue<string>("Issuer");
            var audience    = jwtSettings.GetValue<string>("Audience");
            var expiryMin   = jwtSettings.GetValue<int>("ExpiryMinutes");

       
            var roles = await _userManager.GetRolesAsync(user);

    
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? ""),
            };
      
            foreach (var r in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMin),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
