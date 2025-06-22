using System.Security.Claims;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IUserHelperService
    {
        GenericResponse<Guid?> GetUserIdFromClaims(ClaimsPrincipal user);
    }
}
