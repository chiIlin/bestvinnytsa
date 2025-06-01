using System.Collections.Generic;
using System.Threading.Tasks;
using bestvinnytsa.web.Data.Models;
using bestvinnytsa.web.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace bestvinnytsa.web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICampaignService _campaignService;

		public CategoriesController(ICampaignService campaignService)
		{
			_campaignService = campaignService;
		}

		/// <summary>
		/// Повертає всі доступні категорії.
		/// </summary>
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var categories = await _campaignService.GetCategoriesAsync();
			return Ok(categories);
		}
	}
}
