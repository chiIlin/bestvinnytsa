using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using bestvinnytsa.web.Data.Mongo;
using MongoDB.Driver;
using bestvinnytsa.web.Data.Models;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly MongoContext _mongoContext;

        public ProfileController(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        /// <summary>
        /// Отримати профіль компанії
        /// </summary>
        [Authorize]
        [HttpGet("company")]
        public async Task<IActionResult> GetCompanyProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _mongoContext.Users
                .Find(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { Message = "Користувача не знайдено" });

            return Ok(new
            {
                companyName = user.CompanyName,
                contactPerson = user.ContactPerson,
                email = user.Email,
                companyPhone = user.CompanyPhone,
                website = user.Website,
                industry = user.Industry,
                companySize = user.CompanySize,
                companyDescription = user.CompanyDescription,
                budgetRange = user.BudgetRange,
                targetAudience = user.TargetAudience
            });
        }

        /// <summary>
        /// Оновити профіль компанії
        /// </summary>
        [Authorize]
        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompanyProfile([FromBody] UpdateCompanyProfileRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, userId);
            var update = Builders<AppUser>.Update
                .Set(u => u.CompanyName, request.CompanyName?.Trim())
                .Set(u => u.ContactPerson, request.ContactPerson?.Trim())
                .Set(u => u.CompanyPhone, request.CompanyPhone?.Trim())
                .Set(u => u.Website, request.Website?.Trim())
                .Set(u => u.Industry, request.Industry?.Trim())
                .Set(u => u.CompanySize, request.CompanySize?.Trim())
                .Set(u => u.CompanyDescription, request.CompanyDescription?.Trim())
                .Set(u => u.BudgetRange, request.BudgetRange?.Trim())
                .Set(u => u.TargetAudience, request.TargetAudience?.Trim());

            await _mongoContext.Users.UpdateOneAsync(filter, update);

            return NoContent();
        }

        /// <summary>
        /// Отримати профіль інфлюенсера
        /// </summary>
        [Authorize]
        [HttpGet("influencer")]
        public async Task<IActionResult> GetInfluencerProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _mongoContext.Users
                .Find(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { Message = "Користувача не знайдено" });

            return Ok(new
            {
                fullName = user.FullName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                city = user.City,
                biography = user.Biography,
                contentCategories = user.ContentCategories,
                instagramHandle = user.InstagramHandle,
                youtubeHandle = user.YoutubeHandle,
                tiktokHandle = user.TiktokHandle,
                telegramHandle = user.TelegramHandle
            });
        }

        /// <summary>
        /// Оновити профіль інфлюенсера
        /// </summary>
        [Authorize]
        [HttpPut("influencer")]
        public async Task<IActionResult> UpdateInfluencerProfile([FromBody] UpdateInfluencerProfileRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, userId);
            var update = Builders<AppUser>.Update
                .Set(u => u.FullName, request.FullName?.Trim())
                .Set(u => u.PhoneNumber, request.PhoneNumber?.Trim())
                .Set(u => u.City, request.City?.Trim())
                .Set(u => u.Biography, request.Biography?.Trim())
                .Set(u => u.ContentCategories, request.ContentCategories?.Trim())
                .Set(u => u.InstagramHandle, request.InstagramHandle?.Trim())
                .Set(u => u.YoutubeHandle, request.YoutubeHandle?.Trim())
                .Set(u => u.TiktokHandle, request.TiktokHandle?.Trim())
                .Set(u => u.TelegramHandle, request.TelegramHandle?.Trim());

            await _mongoContext.Users.UpdateOneAsync(filter, update);

            return NoContent();
        }
    }

    // DTOs для запитів
    public class UpdateCompanyProfileRequest
    {
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? CompanyPhone { get; set; }
        public string? Website { get; set; }
        public string? Industry { get; set; }
        public string? CompanySize { get; set; }
        public string? CompanyDescription { get; set; }
        public string? BudgetRange { get; set; }
        public string? TargetAudience { get; set; }
    }

    public class UpdateInfluencerProfileRequest
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Biography { get; set; }
        public string? ContentCategories { get; set; }
        public string? InstagramHandle { get; set; }
        public string? YoutubeHandle { get; set; }
        public string? TiktokHandle { get; set; }
        public string? TelegramHandle { get; set; }
    }
}