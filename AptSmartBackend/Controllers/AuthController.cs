using AptSmartBackend.DTOs;
using AptSmartBackend.Services;
using AptSmartBackend.Services.Abstract;
using AptSmartBackend.SettingsObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AptSmartBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        public AuthController(IConfiguration configuration, IAuthService authService)
        {
            _jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>() ?? throw new KeyNotFoundException("Missing jwt settings");
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userInfo)
        {
            GenericResponse<string> response = await _authService.Register(userInfo);

            if (!response.Success)
            {
                switch (response.StatusCode)
                {
                    case GenericStatusCode.UserAlreadyExists:
                        return StatusCode(400);
                }
            }

            return Ok(response.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userInfo)
        {
            GenericResponse<string> response = await _authService.Login(userInfo);

            if (!response.Success)
            {
                return StatusCode(400);
            }

            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // CHANGE IN PRODUCTION 
                SameSite = SameSiteMode.None, // CHANGE IN PRODUCTIONS
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes)
            };

            Response.Cookies.Append("AuthToken", response.Data, cookieOptions);

            return Ok(response.Message);
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("AuthToken");

            return Ok("User logged out");
        }

        [HttpGet("test")]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            var claims = string.Join(", ", User.Claims.Select(c => c.ToString()));

            return Ok(new { success= true, data=claims });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            string emailClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            string idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;

            if (emailClaim == null)
            {
                return NotFound("Email not found");
            }

            return Ok(new { email=emailClaim, id=idClaim });
        }
    }
}
