using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ApptSmartBackend.Utilities
{
    /// <summary>
    /// Action filter that validates an anti-CSRF token.
    /// Ensures that for state-changing HTTP methods (POST, PUT, PATCH, DELETE),
    /// a matching token is present in both the request headers and cookies.
    /// 
    /// This filter helps protect against Cross-Site Request Forgery Attacks.
    /// Should be applied to controller actions that require CSRF protection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateCsrfTokenAttribute : Attribute, IAsyncActionFilter
    {
        private static readonly string[] MethodsRequiringCsrf = { "POST", "PUT", "PATCH", "DELETE" };
        /// <summary>
        /// Checks the request method and, if necessary, validates the CSRF token.
        /// Skips validation for safe methods like GET or HEAD
        /// </summary>
        /// <param name="context">The context for the current executing action</param>
        /// <param name="next">Delegate to execute the next action filter or action</param>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;

            if (!MethodsRequiringCsrf.Contains(request.Method.ToUpperInvariant()))
            {
                await next();
                return;
            }

            var headerToken = request.Headers["X-CSRF-TOKEN"].FirstOrDefault();
            var cookieToken = request.Cookies["XSRF-TOKEN"];

            if (string.IsNullOrEmpty(headerToken) || string.IsNullOrEmpty(cookieToken) || headerToken != cookieToken)
            {
                context.Result = new ForbidResult(); // Reject the request with 403 Forbidden
                return;
            }

            await next();
        }
    }
}
