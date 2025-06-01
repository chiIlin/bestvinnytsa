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
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        /// <summary>
        /// Створює нову заявку на участь у кампанії. 
        /// Поточний інфлюенсер (userId з JWT) автоматично підставляється в поле InfluencerId.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Application newApplication)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            newApplication.InfluencerId = userId;
            await _applicationService.CreateAsync(newApplication);
            return CreatedAtAction(nameof(GetById), new { id = newApplication.Id }, newApplication);
        }

        /// <summary>
        /// Повертає заявку за Id.
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var application = await _applicationService.GetByIdAsync(id);
            if (application == null)
                return NotFound(new { Message = $"Заявку з Id = {id} не знайдено." });

            return Ok(application);
        }

        /// <summary>
        /// Повертає всі заявки поточного інфлюенсера.
        /// </summary>
        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetByInfluencer()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var apps = await _applicationService.GetByInfluencerAsync(userId);
            return Ok(apps);
        }

        /// <summary>
        /// Повертає всі заявки для даної кампанії (доступно всім, або можна обмежити до власника кампанії).
        /// </summary>
        [Authorize]
        [HttpGet("campaign/{campaignId}")]
        public async Task<IActionResult> GetByCampaign(string campaignId)
        {
            var apps = await _applicationService.GetByCampaignAsync(campaignId);
            return Ok(apps);
        }

        /// <summary>
        /// Оновлює заявку (наприклад, лише поле ContactInfo — решту не міняємо тут).
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Application updatedApplication)
        {
            var existing = await _applicationService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Заявку з Id = {id} не знайдено." });

            // Дозволяємо змінювати лише ContactInfo (як приклад)
            existing.ContactInfo = updatedApplication.ContactInfo;

            await _applicationService.UpdateAsync(existing);
            return NoContent();
        }

        /// <summary>
        /// Встановлює новий статус заявки (наприклад, "Accepted", "Rejected").
        /// </summary>
        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> SetStatus(string id, [FromBody] string status)
        {
            var existing = await _applicationService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Заявку з Id = {id} не знайдено." });

            await _applicationService.SetStatusAsync(id, status);
            return NoContent();
        }

        /// <summary>
        /// Видаляє заявку (DeleteOne).
        /// </summary>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _applicationService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Заявку з Id = {id} не знайдено." });

            await _applicationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
