using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Services;

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
        public async Task<ActionResult<List<Campaign>>> GetOpen()
        {
            var list = await _campaignService.GetOpenCampaignsAsync();
            return Ok(list);
        }

 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Campaign>> GetById(int id)
        {
            var campaign = await _campaignService.GetByIdAsync(id);
            if (campaign == null)
                return NotFound();
            return Ok(campaign);
        }

  
        [HttpGet("producer/{producerId:int}")]
        public async Task<ActionResult<List<Campaign>>> GetByProducer(int producerId)
        {
            var list = await _campaignService.GetByProducerAsync(producerId);
            return Ok(list);
        }

)
        [HttpGet("categories")]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var list = await _campaignService.GetAllCategoriesAsync();
            return Ok(list);
        }


        [HttpPost]
        public async Task<ActionResult> Create([FromBody] Campaign newCampaign)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            newCampaign.CreatedAt = DateTime.UtcNow;
 
            await _campaignService.CreateAsync(newCampaign);
  
            return CreatedAtAction(nameof(GetById), new { id = newCampaign.Id }, newCampaign);
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] Campaign updateCampaign)
        {
            if (id != updateCampaign.Id)
                return BadRequest("ID не співпадає.");

            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();


            existing.Name = updateCampaign.Name;
            existing.Budget = updateCampaign.Budget;
            existing.Description = updateCampaign.Description;
            existing.Link = updateCampaign.Link;
            existing.CategoryId = updateCampaign.CategoryId;
 

            await _campaignService.UpdateAsync(existing);
            return NoContent();
        }


        [HttpPatch("{id:int}/close")]
        public async Task<ActionResult> Close(int id)
        {
            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _campaignService.CloseAsync(id);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existing = await _campaignService.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _campaignService.CloseAsync(id);
            return NoContent();
        }
    }
}
