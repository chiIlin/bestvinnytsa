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
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("campaign/{campaignId:int}")]
        public async Task<ActionResult<List<Application>>> GetByCampaign(int campaignId)
        {
            var list = await _applicationService.GetByCampaignAsync(campaignId);
            return Ok(list);
        }

        [HttpGet("influencer/{influencerId:int}")]
        public async Task<ActionResult<List<Application>>> GetByInfluencer(int influencerId)
        {
            var list = await _applicationService.GetByInfluencerAsync(influencerId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<ActionResult> Apply([FromBody] Application newApplication)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            newApplication.CreatedAt = DateTime.UtcNow;
            newApplication.Status = ApplicationStatus.Pending;
            await _applicationService.ApplyAsync(newApplication);
            return CreatedAtAction(
                nameof(GetById), 
                new { id = newApplication.Id }, 
                newApplication);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Application>> GetById(int id)
        {
            var all = await _applicationService.GetByCampaignAsync(0);
            var app = all.Find(a => a.Id == id);
            if (app == null)
                return NotFound();
            return Ok(app);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<ActionResult> UpdateStatus(int id, [FromQuery] ApplicationStatus status)
        {
            var all = await _applicationService.GetByCampaignAsync(0);
            var existing = all.Find(a => a.Id == id);
            if (existing == null)
                return NotFound();

            await _applicationService.UpdateStatusAsync(id, status);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var all = await _applicationService.GetByCampaignAsync(0);
            var existing = all.Find(a => a.Id == id);
            if (existing == null)
                return NotFound();

            return NoContent();
        }
    }
}