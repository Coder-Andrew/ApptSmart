using ApptSmartBackend.DTOs;
using System.Security.Claims;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IAuthService
    {
        Task<GenericResponse<string>> Login(LoginDto loginInfo);
        Task<GenericResponse<string>> Register(RegisterDto registerInfo);
        Task<GenericResponse<string>> Logout();
        GenericResponse<UserInfoDto> GetUserInfo(ClaimsPrincipal user);
    }
}
