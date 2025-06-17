using ApptSmartBackend.Utilities;
using ApptSmartBackend.Services.Concrete;
using Microsoft.AspNetCore.Mvc;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Controllers
{
    [Route("api/company/{companySlug}")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
        {
            _companyService = companyService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetCompanyInfo(string companySlug)
        {
            try
            {
                var company = await _companyService.GetCompanyAsync(companySlug);

                if (company == null)
                {
                    return NotFound("Company not found");
                }

                var dto = new CompanyDto
                {
                    CompanyName = company.CompanyName,
                    CompanyDescription = company.CompanyDescription ?? string.Empty
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while processing Get()");
                return StatusCode(500, ErrorMessages.Generic);
            }
        }
    }
}
