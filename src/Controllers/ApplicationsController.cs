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

        [Authorize]
        [HttpGet("campaign/{campaignId:int}")]
        public async Task<IActionResult> GetByCampaign(int campaignId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var applications = await _applicationService.GetByCampaignAsync(campaignId);
            return Ok(applications);
        }

        [Authorize]
        [HttpGet("influencer/my")]
        public async Task<IActionResult> GetByInfluencer()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var applications = await _applicationService.GetByInfluencerAsync(userId);
            return Ok(applications);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Application newApplication)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            newApplication.InfluencerId = userId;
            await _applicationService.ApplyAsync(newApplication);
            return CreatedAtAction(nameof(GetByCampaign), new { campaignId = newApplication.CampaignId }, newApplication);
        }

        [Authorize]
        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] ApplicationStatus status)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _applicationService.UpdateStatusAsync(id, status);
            return NoContent();
        }
    }
}
