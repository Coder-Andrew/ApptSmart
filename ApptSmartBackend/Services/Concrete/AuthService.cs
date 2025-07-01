using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models;
using ApptSmartBackend.Helpers;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models.AppModels;
using Microsoft.Extensions.Options;
using ApptSmartBackend.SettingsObjects;

namespace ApptSmartBackend.Services.Concrete
{
    /// <summary>
    /// Provides authentication-related functionality including login, registration,
    /// user info retrieval, and refresh token management
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IUserInfoRepositoryAsync _appUserRepository;
        private readonly IRefreshTokenService _refreshTokenService;

        // TODO: Move to an architecture/service layer that returns/handles models directly
        // TODO: Add logging for error reporting, auth events, etc.
        public AuthService(UserManager<AuthUser> userManager, IUserInfoRepositoryAsync appUserRepo, JwtHelper jwtHelper, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _appUserRepository = appUserRepo;
            _refreshTokenService = refreshTokenService;
        }

        /// <summary>
        /// Extracts and returns application-specific user information from the provided claims.
        /// </summary>
        /// <param name="user">The <see cref="ClaimsPrincipal"/> associated with the current user context.</param>
        /// <returns>
        /// A <see cref="GenericResponse{UserInfoDto}"/> containing the user's ID, email, and roles.
        /// Returns a failure response if required claims are missing.
        /// </returns>
        public GenericResponse<UserInfoDto> GetUserInfo(ClaimsPrincipal user)
        {
            // TODO: Find out a better way to pass message around
            string? emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string? idClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string[] roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value.ToString()).ToArray();

            string[] missingClaims = new List<string>
            {
                string.IsNullOrEmpty(emailClaim) ? "Email" : null,
                string.IsNullOrEmpty(idClaim) ? "Id" : null
            }.Where(s => s != null).ToArray();

            if (missingClaims.Any())
            {
                return new GenericResponse<UserInfoDto>
                (
                    data: null,
                    success: false,
                    message: $"Missing claims: {string.Join(", ", missingClaims)}",
                    statusCode: GenericStatusCode.Failure
                );
            }            

            return new GenericResponse<UserInfoDto>
            (
                data: new UserInfoDto
                {
                    Email = emailClaim!,
                    Id = idClaim!,
                    Roles = roles
                },
                success: true,
                message: "Successfully retrieved user info",
                statusCode: GenericStatusCode.Success
            );
        }

        /// <summary>
        /// Attempts to authenticate a user with provided credentials and issues JWT + refresh token if successful.
        /// </summary>
        /// <param name="loginInfo">The user's login credentials including email and password.</param>
        /// <returns>
        /// A <see cref="GenericResponse{AuthResponseDto}"/> containing the generated tokens if successful
        /// or an error message on failure
        /// </returns>
        public async Task<GenericResponse<AuthResponseDto>> Login(LoginDto loginInfo)
        {
            AuthUser? user = await _userManager.FindByEmailAsync(loginInfo.Email);

            if (user == null || ! await _userManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                return new GenericResponse<AuthResponseDto>(
                    data: new AuthResponseDto(),
                    success: false,
                    message: "Invalid credentials",
                    statusCode: GenericStatusCode.InvalidCredentials
                );
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var jwt = _jwtHelper.GenerateJwt(user, userRoles);

            var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(user);            

            return new GenericResponse<AuthResponseDto>(
                data: new AuthResponseDto
                {
                    Jwt = jwt,
                    CsrfToken = _jwtHelper.GenerateUrlSafeToken(),
                    RefreshToken = refreshToken,
                },
                success: true,
                message: "Login successful",
                statusCode: GenericStatusCode.Success
            );
        }

        public Task<GenericResponse<string>> Logout()
        {
            // Might be a good place for logging
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the registration of a new user. Creates a new <see cref="AuthUser"/> and <see cref="UserInfo"/> model and saves in to their respecive databases.
        /// </summary>
        /// <param name="registerInfo">The registration information including email, password, and name.</param>
        /// <returns>
        /// A <see cref="GenericResponse{string}"/> containing the user's identity ID if successful,
        /// or an error code/message on failure.
        /// </returns>
        public async Task<GenericResponse<string>> Register(RegisterDto registerInfo)
        {
            // TODO: Handle errors better and add transaction scoping
            if (await _userManager.FindByEmailAsync(registerInfo.Email) != null)
            {
                return new GenericResponse<string>(
                    data: null,
                    success: false,
                    message: "User already exists",
                    statusCode: GenericStatusCode.UserAlreadyExists
                );
            }

            AuthUser user = new AuthUser
            {
                UserName = registerInfo.Email,
                Email = registerInfo.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerInfo.Password);
            

            if (!result.Succeeded)
            {
                return new GenericResponse<string>(
                    data: null,
                    success: false,
                    message: "User creation failed",
                    statusCode: GenericStatusCode.FailedToCreateUser
                );
            }

            // TODO: potentially move role handling off into its own method
            IdentityResult roleResult = await _userManager.AddToRoleAsync(user, "user");

            if (!roleResult.Succeeded)
            {
                return new GenericResponse<string>(
                    data: null,
                    success: false,
                    message: "Failed to add role to user",
                    statusCode: GenericStatusCode.FailedToAddRole
                );
            }

            // TODO: Handle errors when commiting to changes to this db fails
            await _appUserRepository.AddOrUpdateAsync(new UserInfo
            {
                AspNetIdentityId = user.Id,
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName,
            });

            return new GenericResponse<string>(
                data: user.Id,
                success: true,
                message: "User created successfully",
                statusCode: GenericStatusCode.UserCreated
            );
        }
    }
}
