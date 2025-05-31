using System.Threading.Tasks;
using bestvinnytsa.web.Data.DTOs;
using bestvinnytsa.web.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace bestvinnytsa.web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Реєстрація нового користувача (Producer або Influencer). 
        /// Повертає JWT-токен.
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.RegisterAsync(request);
                return Ok(new { Token = token });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Вхід користувача. Повертає JWT-токен.
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.LoginAsync(request);
                return Ok(new { Token = token });
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }
    }
}
