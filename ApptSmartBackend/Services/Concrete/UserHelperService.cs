using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Services.Abstract;
using System.Security.Claims;

namespace ApptSmartBackend.Services.Concrete
{
    // TODO: If userid lookup becomes a bottleneck, include it in JWT or introduce a cache
    public class UserHelperService : IUserHelperService
    {
        private readonly IUserInfoRepositoryAsync _userInfoRepositoryAsync;
        private readonly ILogger<UserHelperService> _logger;
        public UserHelperService(IUserInfoRepositoryAsync userInfoRepositoryAsync, ILogger<UserHelperService> logger)
        {
            _userInfoRepositoryAsync = userInfoRepositoryAsync;
            _logger = logger;
        }

        public GenericResponse<Guid?> GetUserIdFromClaims(ClaimsPrincipal user)
        {
            string? userAspNetId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;            

            if (string.IsNullOrEmpty(userAspNetId))
            {
                _logger.LogWarning("Unauthorized access attempt: User claim is missing an ASP.NET Identity ID.");
                return new GenericResponse<Guid?> 
                (
                    data: null,
                    message: "Unauthorized: No ASP.NET Identity ID found.",
                    success: false,
                    statusCode: GenericStatusCode.FailedToGetUserAspNetClaim
                );
            }

            Guid? userAppId = _userInfoRepositoryAsync.GetUserId(userAspNetId);

            if (!userAppId.HasValue)
            {
                _logger.LogWarning($"Cannot find App User Id for user with ASP.NET Identity Id: {userAspNetId}");
                return new GenericResponse<Guid?>
                (
                    data: null,
                    success: false,
                    message: "User not found",
                    statusCode: GenericStatusCode.FailedToGetUserInfoId
                );
            }

            return new GenericResponse<Guid?>
            (
                data: userAppId,
                success: true,
                message: "Successfully found UserInfoId",
                GenericStatusCode.Success
            );
        }
    }
}
