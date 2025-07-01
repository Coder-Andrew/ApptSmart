using ApptSmartBackend.DTOs;
using ApptSmartBackend.Helpers;
using ApptSmartBackend.Models;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.SettingsObjects;
using ApptSmartBackend.Utilities;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApptSmartBackend.Controllers
{
    /// <summary>
    /// Handles user login and token issuance.
    /// </summary>
    /// <remarks>Look into moving a lot of this functionality off into the auth service</remarks>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly JwtHelper _jwtHelper;
        private readonly UserManager<AuthUser> _userManager;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            IOptions<JwtSettings> options, 
            IAuthService authService, 
            IRefreshTokenService refreshTokenService, 
            JwtHelper jwtHelper, 
            UserManager<AuthUser> userManager,
            ILogger<AuthController> logger)
        {
            _jwtSettings = options.Value;
            _authService = authService;
            _refreshTokenService = refreshTokenService;
            _jwtHelper = jwtHelper;
            _userManager = userManager;
            _logger = logger;
        }

        private void SetAuthCookies(string jwt, string csrfToken, RefreshToken refreshToken)
        {
            CookieOptions jwtCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes)
            };

            CookieOptions refreshTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.RefreshExpiryMinutes)
            };

            CookieOptions csrfCookieOptions = new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = refreshToken.Expires,
            };

            Response.Cookies.Append("AuthToken", jwt, jwtCookieOptions);
            Response.Cookies.Append("XSRF-TOKEN", csrfToken, csrfCookieOptions);
            Response.Cookies.Append("AuthRefreshToken", refreshToken.Token, refreshTokenOptions);
        }

        /// <summary>
        /// Registers a user
        /// </summary>
        /// <param name="userInfo">The credentials a user registers with.</param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto userInfo)
        {
            try
            {
                GenericResponse<string> response = await _authService.Register(userInfo);

                if (!response.Success)
                {
                    return response.StatusCode switch
                    {
                        GenericStatusCode.UserAlreadyExists => BadRequest("User already exists"),
                        _ => StatusCode(500, "Unexpected error")
                    };
                }

                return Ok(response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when registering user with email: {userInfo.Email}");
                return StatusCode(500, ErrorMessages.Generic);
            }
        }

        /// <summary>
        /// Handles user login and JWT issuance.
        /// </summary>
        /// <param name="userInfo">The user's login credentials</param>
        /// <returns>JWT token cookie on success; Unauthorized on failure.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userInfo)
        {
            // ERROR HERE, TOKEN IS NULL
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

            if (response.Data!.RefreshToken == null)
            {
                _logger.LogError("Login succeeded but refresh token is null");
                return StatusCode(500, ErrorMessages.Generic);
            }

            SetAuthCookies(response.Data!.Jwt, response.Data.CsrfToken, response.Data.RefreshToken);

            // Change later for third-party consumers
            return Ok(response.Message);
        }

        /// <summary>
        /// Handles logging out a user
        /// </summary>
        /// <returns>A 200 OK response with auth and refresh token deletion</returns>
        /// <remarks>Will handle logging later.</remarks>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["AuthRefreshToken"];
            if (!String.IsNullOrEmpty(refreshToken))
            {
                await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
            }

            Response.Cookies.Delete("AuthRefreshToken");
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("XSRF-TOKEN");

            return Ok("User logged out");
        }

        [ValidateCsrfToken]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            // This method is getting a little bloated, might want to move to a service
            try
            {
                // Check cookies for refresh token, return unauthorized if not found.
                var refreshCookie = Request.Cookies["AuthRefreshToken"];
                if (refreshCookie == null) return Unauthorized("Missing refresh token");

                // Get refresh token model
                var oldToken = await _refreshTokenService.GetValidRefreshTokenAsync(refreshCookie);
                if (oldToken == null) return Unauthorized("Invalid or expired refresh token");

                // Revoke old token
                await _refreshTokenService.RevokeRefreshTokenAsync(oldToken.Token);

                // Create new, valid token, with token information from previous token
                RefreshToken newToken = new RefreshToken
                {
                    Token = _jwtHelper.GenerateUrlSafeToken(64),
                    UserId = oldToken.UserId,
                    Created = DateTime.UtcNow,
                    Expires = oldToken.Expires,
                    IsRevoked = false,
                };

                // Save new token
                await _refreshTokenService.SaveRefreshToken(newToken);

                // Generate new jwt based on user's info
                var user = oldToken.User;
                var roles = await _userManager.GetRolesAsync(user);
                var jwt = _jwtHelper.GenerateJwt(user, roles);

                // Set auth values as cookies; generate new csrf token
                SetAuthCookies(jwt, _jwtHelper.GenerateUrlSafeToken(), newToken);

                return Ok(new { message = "Token refreshed"});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error has occured when refreshing token");
                return StatusCode(500, ErrorMessages.Generic);
            }

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
