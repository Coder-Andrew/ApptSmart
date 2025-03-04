using ApptSmartBackend.DTOs;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.SettingsObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ApptSmartBackend.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        public AuthController(IOptions<JwtSettings> options, IAuthService authService)
        {
            _jwtSettings = options.Value;
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
            GenericResponse<UserInfoDto> res = _authService.GetUserInfo(User);

            if (!res.Success)
            {
                return NotFound(new { success = false, message = res.Message });
            }

            UserInfoDto userInfo = res.Data!;

            return Ok(userInfo);
        }
    }
}
