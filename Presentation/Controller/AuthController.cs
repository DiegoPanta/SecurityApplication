using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Presentation.Interfaces;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("request-login")]
        public async Task<IActionResult> RequestLogin([FromBody] LoginDto request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            var token = await _authService.RequestLoginAsync(request.Email);
            if (!string.IsNullOrEmpty(token))
                return Ok(new { token });
            //return Ok("Login link sent to your email.");
            return BadRequest("Failed to send login link.");
        }

        [HttpGet("verify-login")]
        public IActionResult VerifyLogin([FromQuery] string token)
        {
            var isValid = _authService.VerifyLogin(token);
            if (isValid)
                return Ok(new { Token = token });
            return Unauthorized();
        }

        [Authorize]
        [HttpGet("protected")]
        public IActionResult ProtectedEndpoint()
        {
            return Ok("You have access to this protected endpoint!");
        }
    }
}
