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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var token = await _authService.RegisterAndGenerateTokenAsync(request);
                return Ok(new { Token = token });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var token = await _authService.LoginAndGenerateTokenAsync(request);
                return Ok(new { Token = token });
            }
            catch (System.Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
}