using ApptSmartBackend.DTOs;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.SettingsObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApptSmartBackend.Controllers
{
    /// <summary>
    /// Handles user login and token issuance.
    /// </summary>
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

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userInfo">The credentials a user registers with.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Handles user login and JWT issuance.
        /// </summary>
        /// <param name="userInfo">The user's login credentials</param>
        /// <returns>JWT token cookie on success; Unauthorized on failure.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userInfo)
        {
            GenericResponse<AuthResponseDto> response = await _authService.Login(userInfo);

            if (!response.Success)
            {
                switch (response.StatusCode) 
                {
                    case GenericStatusCode.InvalidCredentials:
                        return StatusCode(401);
                    default:
                        return BadRequest();
                }
            }

            CookieOptions jwtCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None, 
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes)
            };

            CookieOptions csrfCookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpiryMinutes) // TODO: Change later, to something longer
            };

            Response.Cookies.Append("AuthToken", response.Data!.Jwt, jwtCookieOptions);
            Response.Cookies.Append("XSRF-TOKEN", response.Data!.CsrfToken, csrfCookieOptions);

            // Change later for third-party consumers
            return Ok(response.Message);
        }

        /// <summary>
        /// Handles logging out a user
        /// </summary>
        /// <returns>A 200 OK response with auth token deletion</returns>
        /// <remarks>Will handle logging later.</remarks>
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

        /// <summary>
        /// Issues updated user info
        /// </summary>
        /// <returns>UserInfo containing user's information</returns>
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
