using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using bestvinnytsa.web.Data.Mongo;
using bestvinnytsa.web.Data.Models;
using MongoDB.Driver;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Поки що просто авторизовані користувачі, пізніше можна додати роль Admin
    public class AdminController : ControllerBase
    {
        private readonly MongoContext _mongoContext;

        public AdminController(MongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        /// <summary>
        /// Отримати статистику платформи
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                // Загальна кількість користувачів
                var totalUsers = await _mongoContext.Users.CountDocumentsAsync(_ => true);
                
                // Кількість інфлюенсерів
                var influencersCount = await _mongoContext.Users
                    .CountDocumentsAsync(u => u.Roles.Contains("Person"));
                
                // Кількість брендів
                var brandsCount = await _mongoContext.Users
                    .CountDocumentsAsync(u => u.Roles.Contains("Company"));
                
                // Кількість кампаній
                var campaignsCount = await _mongoContext.Campaigns.CountDocumentsAsync(_ => true);
                
                // Кількість заявок
                var applicationsCount = await _mongoContext.Applications.CountDocumentsAsync(_ => true);
                
                // Топ міст
                var topCities = await _mongoContext.Users
                    .Aggregate()
                    .Match(u => !string.IsNullOrEmpty(u.City))
                    .Group(u => u.City, g => new { City = g.Key, Count = g.Count() })
                    .SortByDescending(x => x.Count)
                    .Limit(5)
                    .ToListAsync();

                return Ok(new
                {
                    totalUsers,
                    influencersCount,
                    brandsCount,
                    campaignsCount,
                    applicationsCount,
                    topCities = topCities.Select(x => new { city = x.City, count = x.Count })
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка отримання статистики", error = ex.Message });
            }
        }

        /// <summary>
        /// Отримати список всіх користувачів з пагінацією
        /// </summary>
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 10, [FromQuery] string? search = null, [FromQuery] string? role = null)
        {
            try
            {
                var skip = (page - 1) * limit;
                var filter = Builders<AppUser>.Filter.Empty;

                // Фільтр по ролі
                if (!string.IsNullOrEmpty(role))
                {
                    filter = filter & Builders<AppUser>.Filter.AnyEq(u => u.Roles, role);
                }

                // Пошук по імені або email
                if (!string.IsNullOrEmpty(search))
                {
                    var searchFilter = Builders<AppUser>.Filter.Or(
                        Builders<AppUser>.Filter.Regex(u => u.FullName, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                        Builders<AppUser>.Filter.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression(search, "i")),
                        Builders<AppUser>.Filter.Regex(u => u.CompanyName, new MongoDB.Bson.BsonRegularExpression(search, "i"))
                    );
                    filter = filter & searchFilter;
                }

                var totalCount = await _mongoContext.Users.CountDocumentsAsync(filter);

                var users = await _mongoContext.Users
                    .Find(filter)
                    .Skip(skip)
                    .Limit(limit)
                    .Project(u => new
                    {
                        id = u.Id,
                        fullName = u.FullName,
                        companyName = u.CompanyName,
                        email = u.Email,
                        roles = u.Roles,
                        city = u.City,
                        isBlocked = u.IsBlocked ?? false,
                        photoUrl = u.PhotoUrl,
                        instagramFollowers = u.InstagramFollowers,
                        industry = u.Industry
                    })
                    .ToListAsync();

                return Ok(new
                {
                    users,
                    totalCount,
                    page,
                    totalPages = (int)Math.Ceiling((double)totalCount / limit)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка отримання користувачів", error = ex.Message });
            }
        }

        /// <summary>
        /// Заблокувати/розблокувати користувача
        /// </summary>
        [HttpPut("users/{id}/block")]
        public async Task<IActionResult> ToggleUserBlock(string id, [FromBody] BlockUserRequest request)
        {
            try
            {
                var filter = Builders<AppUser>.Filter.Eq(u => u.Id, id);
                var update = Builders<AppUser>.Update.Set(u => u.IsBlocked, request.IsBlocked);
                
                var result = await _mongoContext.Users.UpdateOneAsync(filter, update);
                
                if (result.MatchedCount == 0)
                {
                    return NotFound(new { message = "Користувача не знайдено" });
                }

                return Ok(new { message = request.IsBlocked ? "Користувача заблоковано" : "Користувача розблоковано" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка блокування користувача", error = ex.Message });
            }
        }

        /// <summary>
        /// Видалити користувача
        /// </summary>
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var result = await _mongoContext.Users.DeleteOneAsync(u => u.Id == id);
                
                if (result.DeletedCount == 0)
                {
                    return NotFound(new { message = "Користувача не знайдено" });
                }

                return Ok(new { message = "Користувача видалено" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка видалення користувача", error = ex.Message });
            }
        }
    }

    public class BlockUserRequest
    {
        public bool IsBlocked { get; set; }
    }
}