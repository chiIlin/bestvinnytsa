using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        /// <summary>
        /// Повертає всі відкриті кампанії (IsOpen = true).
        /// </summary>
        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            var campaigns = await _campaignService.GetOpenCampaignsAsync();
            return Ok(campaigns);
        }

        /// <summary>
        /// Повертає конкретну кампанію за Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var campaign = await _campaignService.GetByIdAsync(id);
            if (campaign == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            return Ok(campaign);
        }

        /// <summary>
        /// Повертає всі кампанії поточного продюсера.
        /// </summary>
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetByProducer()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var campaigns = await _campaignService.GetByProducerAsync(userId);
            return Ok(campaigns);
        }

        /// <summary>
        /// Cтворює нову кампанію. 
        /// Id нового документу формується Mongo; Producing user визначається з JWT.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Campaign newCampaign)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Перевіряємо обов'язкові поля
            if (string.IsNullOrEmpty(newCampaign.Name))
                return BadRequest(new { Message = "Name is required" });
                
            if (string.IsNullOrEmpty(newCampaign.Description))
                return BadRequest(new { Message = "Description is required" });

            // Встановлюємо ProducerId з JWT токена
            newCampaign.ProducerId = userId;
            
            // Встановлюємо значення за замовчуванням
            if (newCampaign.Budget <= 0)
                newCampaign.Budget = 1000;
                
            if (string.IsNullOrEmpty(newCampaign.Link))
                newCampaign.Link = "";

            try
            {
                await _campaignService.CreateAsync(newCampaign);
                return CreatedAtAction(nameof(GetById), new { id = newCampaign.Id }, newCampaign);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error creating campaign", Error = ex.Message });
            }
        }

        /// <summary>
        /// Оновлює наявну кампанію (лише якщо вона належить поточному продюсеру).
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Campaign updatedCampaign)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            if (existing.ProducerId != userId)
                return Forbid();

            // Оновлюємо лише дозволені поля
            existing.Name = updatedCampaign.Name;
            existing.Budget = updatedCampaign.Budget;
            existing.Description = updatedCampaign.Description;
            existing.Link = updatedCampaign.Link;
            existing.CategoryId = updatedCampaign.CategoryId;
            existing.IsOpen = updatedCampaign.IsOpen;
            existing.ExpiresAt = updatedCampaign.ExpiresAt;

            await _campaignService.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Закриває кампанію (ставить IsOpen = false). 
        /// Доступно лише власнику (producerId==userId).
        /// </summary>
        [Authorize]
        [HttpPatch("{id}/close")]
        public async Task<IActionResult> Close(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            if (existing.ProducerId != userId)
                return Forbid();

            await _campaignService.CloseAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Видаляє кампанію (DeleteOne). Доступно лише власнику (producerId==userId).
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            if (existing.ProducerId != userId)
                return Forbid();

            await _campaignService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Повертає всі доступні категорії.
        /// </summary>
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _campaignService.GetCategoriesAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Помилка отримання категорій", error = ex.Message });
            }
        }
    }
}
