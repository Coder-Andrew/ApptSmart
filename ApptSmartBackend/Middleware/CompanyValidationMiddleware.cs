using ApptSmartBackend.DAL.Abstract;

namespace ApptSmartBackend.Middleware
{
    public class CompanyValidationMiddleware
    {
        private readonly RequestDelegate _next;
        public CompanyValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICompanyRepository companyRepository)
        {
            if (context.Request.RouteValues.TryGetValue("companySlug", out var slugValue))
            {
                var companySlug = slugValue?.ToString();
                if (string.IsNullOrEmpty(companySlug)) {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("Company slug is required");
                    return;
                }

                if (!await companyRepository.ExistsAsync(companySlug))
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Company not found");
                    return;
                };

                context.Items["CompanySlug"] = companySlug;
            }

            await _next(context);
        }
    }
}
