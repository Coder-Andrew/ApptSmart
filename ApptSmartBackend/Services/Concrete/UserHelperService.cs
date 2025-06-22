using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Services.Abstract;
using System.Security.Claims;

namespace ApptSmartBackend.Services.Concrete
{
    /// <summary>
    /// Provides helper methods for retrieving application-specific user information from the current ClaimsPrincipal context.
    /// </summary>
    /// <remarks>
    /// This service is responsible for mapping ASP.NET Identity user claims to application-level user IDs.
    /// Consider caching or token-based solutions if performance becomes a bottleneck.
    /// </remarks>
    public class UserHelperService : IUserHelperService
    {
        private readonly IUserInfoRepositoryAsync _userInfoRepositoryAsync;
        private readonly ILogger<UserHelperService> _logger;
        public UserHelperService(IUserInfoRepositoryAsync userInfoRepositoryAsync, ILogger<UserHelperService> logger)
        {
            _userInfoRepositoryAsync = userInfoRepositoryAsync;
            _logger = logger;
        }

        /// <summary>
        /// Extracts the application-level user ID (UserInfoId) from the given <see cref="ClaimsPrincipal"/>
        /// </summary>
        /// <param name="user">The ClaimsPrincipal representing the currently authenticated user.</param>
        /// <returns>
        /// A <see cref="GenericResponse{T}"/> containing the user's application-specific <see cref="Guid"/> if found;
        /// otherwise, an error message and corresponding status code.
        /// </returns>
        /// <remarks>
        /// This method looks up the ASP.NET Identity ID from the user's claims, then maps it to a UserInfoId via the repository.
        /// Logs a warning if the claim is missing or no matching user is found in the database.
        /// </remarks>
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
