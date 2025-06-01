using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Mongo;
using bestvinnytsa.web.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly MongoContext _mongoContext;
        private readonly IWebHostEnvironment _env;

        public ProfileController(MongoContext mongoContext, IWebHostEnvironment env)
        {
            _mongoContext = mongoContext;
            _env = env;
        }

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
                companyName        = user.CompanyName,
                contactPerson      = user.ContactPerson,
                email              = user.Email,
                companyPhone       = user.CompanyPhone,
                website            = user.Website,
                industry           = user.Industry,
                companySize        = user.CompanySize,
                companyDescription = user.CompanyDescription,
                budgetRange        = user.BudgetRange,
                targetAudience     = user.TargetAudience,
                photoUrl           = user.PhotoUrl
            });
        }

        [Authorize]
        [HttpPut("company")]
        public async Task<IActionResult> UpdateCompanyProfile([FromBody] UpdateCompanyProfileRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, userId);
            var update = Builders<AppUser>.Update
                .Set(u => u.CompanyName,        request.CompanyName?.Trim())
                .Set(u => u.ContactPerson,      request.ContactPerson?.Trim())
                .Set(u => u.CompanyPhone,       request.CompanyPhone?.Trim())
                .Set(u => u.Website,            request.Website?.Trim())
                .Set(u => u.Industry,           request.Industry?.Trim())
                .Set(u => u.CompanySize,        request.CompanySize?.Trim())
                .Set(u => u.CompanyDescription, request.CompanyDescription?.Trim())
                .Set(u => u.BudgetRange,        request.BudgetRange?.Trim())
                .Set(u => u.TargetAudience,     request.TargetAudience?.Trim());

            await _mongoContext.Users.UpdateOneAsync(filter, update);
            return NoContent();
        }

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
                fullName          = user.FullName,
                email             = user.Email,
                phoneNumber       = user.PhoneNumber,
                city              = user.City,
                biography         = user.Biography,
                contentCategories = user.ContentCategories,
                instagramHandle   = user.InstagramHandle,
                youtubeHandle     = user.YoutubeHandle,
                tiktokHandle      = user.TiktokHandle,
                telegramHandle    = user.TelegramHandle,
                photoUrl          = user.PhotoUrl
            });
        }

        [Authorize]
        [HttpPut("influencer")]
        public async Task<IActionResult> UpdateInfluencerProfile([FromBody] UpdateInfluencerProfileRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var filter = Builders<AppUser>.Filter.Eq(u => u.Id, userId);
            var update = Builders<AppUser>.Update
                .Set(u => u.FullName,         request.FullName?.Trim())
                .Set(u => u.PhoneNumber,      request.PhoneNumber?.Trim())
                .Set(u => u.City,             request.City?.Trim())
                .Set(u => u.Biography,        request.Biography?.Trim())
                .Set(u => u.ContentCategories,request.ContentCategories?.Trim())
                .Set(u => u.InstagramHandle,  request.InstagramHandle?.Trim())
                .Set(u => u.YoutubeHandle,    request.YoutubeHandle?.Trim())
                .Set(u => u.TiktokHandle,     request.TiktokHandle?.Trim())
                .Set(u => u.TelegramHandle,   request.TelegramHandle?.Trim());

            await _mongoContext.Users.UpdateOneAsync(filter, update);
            return NoContent();
        }

        [Authorize]
        [HttpPost("influencer/photo")]
        public async Task<IActionResult> UploadInfluencerPhoto([FromForm] IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest(new { Message = "Файл не обрано або він пустий." });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExt))
                return BadRequest(new { Message = "Дозволені формати: .jpg, .jpeg, .png" });

            var user = await _mongoContext.Users
                .Find(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { Message = "Користувача не знайдено." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var newFileName = $"{userId}{fileExt}";
            var filePath = Path.Combine(uploadsFolder, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var relativePath = $"/uploads/{newFileName}";
            var update = Builders<AppUser>.Update.Set(u => u.PhotoUrl, relativePath);
            await _mongoContext.Users.UpdateOneAsync(
                Builders<AppUser>.Filter.Eq(u => u.Id, userId),
                update
            );

            return Ok(new { PhotoUrl = relativePath });
        }

        [Authorize]
        [HttpPost("company/photo")]
        public async Task<IActionResult> UploadCompanyPhoto([FromForm] IFormFile file)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (file == null || file.Length == 0)
                return BadRequest(new { Message = "Файл не обрано або він пустий." });

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExt = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExt))
                return BadRequest(new { Message = "Дозволені формати: .jpg, .jpeg, .png" });

            var user = await _mongoContext.Users
                .Find(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null)
                return NotFound(new { Message = "Користувача не знайдено." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var newFileName = $"company-{userId}{fileExt}";
            var filePath = Path.Combine(uploadsFolder, newFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var relativePath = $"/uploads/{newFileName}";
            var update = Builders<AppUser>.Update.Set(u => u.PhotoUrl, relativePath);
            await _mongoContext.Users.UpdateOneAsync(
                Builders<AppUser>.Filter.Eq(u => u.Id, userId),
                update
            );

            return Ok(new { PhotoUrl = relativePath });
        }
    }

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