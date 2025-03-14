using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ApptSmartBackend.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static ActionResult<Guid> GetUserId(this ControllerBase controller, IUserHelperService userHelperService)
        {
            GenericResponse<Guid?> result = userHelperService.GetUserIdFromClaims(controller.User);

            if (!result.Success || result.Data == null)
            {
                if (result.StatusCode == GenericStatusCode.FailedToGetUserAspNetClaim) return controller.Unauthorized("Invalid credentials");
                return controller.NotFound("User not found");
            }

            return result.Data;
        }
    }
}
