using ApptSmartBackend.Middleware;

namespace ApptSmartBackend.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCompanyValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CompanyValidationMiddleware>();
        }
    }
}
