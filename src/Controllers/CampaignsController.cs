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

     
        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            var campaigns = await _campaignService.GetOpenCampaignsAsync();
            return Ok(campaigns);
        }

        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var campaign = await _campaignService.GetByIdAsync(id);
            if (campaign == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            return Ok(campaign);
        }

        
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

       
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Campaign newCampaign)
        {
           
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

          
            newCampaign.ProducerId = userId;

            await _campaignService.CreateAsync(newCampaign);
            return CreatedAtAction(nameof(GetById), new { id = newCampaign.Id }, newCampaign);
        }

        
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Campaign updatedCampaign)
        {
       
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

           
            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

           
            if (existing.ProducerId != userId)
                return Forbid();

        
            existing.Name        = updatedCampaign.Name;
            existing.Budget      = updatedCampaign.Budget;
            existing.Description = updatedCampaign.Description;
            existing.Link        = updatedCampaign.Link;
            existing.CategoryId  = updatedCampaign.CategoryId;
            existing.IsOpen      = updatedCampaign.IsOpen;
            existing.ExpiresAt   = updatedCampaign.ExpiresAt;

            await _campaignService.UpdateAsync(existing);
            return NoContent();
        }

        
        [Authorize]
        [HttpPatch("{id:int}/close")]
        public async Task<IActionResult> Close(int id)
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

     
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { Message = $"Кампанію з Id = {id} не знайдено." });

            if (existing.ProducerId != userId)
                return Forbid();

           
            await _campaignService.UpdateAsync(existing); 
            return NoContent();
        }
    }
}
