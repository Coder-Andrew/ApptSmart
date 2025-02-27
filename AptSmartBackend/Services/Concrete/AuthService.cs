using AptSmartBackend.DTOs;
using AptSmartBackend.Models;
using AptSmartBackend.Helpers;
using AptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Identity;

namespace AptSmartBackend.Services.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly JwtHelper _jwtHelper;
        public AuthService(UserManager<AuthUser> userManager, JwtHelper jwtHelper)
        {
            _userManager = userManager;
            _jwtHelper = jwtHelper;
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
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName,
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

            return new GenericResponse<string>(
                data: user.Id,
                success: true,
                message: "User created successfully",
                statusCode: GenericStatusCode.UserCreated
            );
        }
    }
}
