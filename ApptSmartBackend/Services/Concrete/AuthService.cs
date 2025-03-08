using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models;
using ApptSmartBackend.Helpers;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Concrete
{
    // TODO: Move to an architecture/service layer that returns/handles models
    public class AuthService : IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IAppUserRepositoryAsync _appUserRepository;
        // TODO: Add logging
        public AuthService(UserManager<AuthUser> userManager, IAppUserRepositoryAsync appUserRepo, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
            _appUserRepository = appUserRepo;
        }

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

        public async Task<GenericResponse<string>> Login(LoginDto loginInfo)
        {
            AuthUser? user = await _userManager.FindByEmailAsync(loginInfo.Email);

            if (user == null || ! await _userManager.CheckPasswordAsync(user, loginInfo.Password))
            {
                return new GenericResponse<string>(
                    data: string.Empty,
                    success: false,
                    message: "Invalid credentials",
                    statusCode: GenericStatusCode.InvalidCredentials
                );
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new GenericResponse<string>(
                data: _jwtHelper.GenerateJwt(user, userRoles),
                success: true,
                message: "Login successful",
                statusCode: GenericStatusCode.Success
            );
        }

        public Task<GenericResponse<string>> Logout()
        {
            throw new NotImplementedException();
        }

        // TODO: Handle errors better and add transaction scoping
        public async Task<GenericResponse<string>> Register(RegisterDto registerInfo)
        {
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
