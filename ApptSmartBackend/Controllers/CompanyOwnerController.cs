using ApptSmartBackend.DTOs;
using ApptSmartBackend.Extensions;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApptSmartBackend.Controllers
{
    [Authorize]
    [Authorize(Roles = "CompanyOwner")]
    [Route("api/companies/{companySlug}/owner")]
    public class CompanyOwnerController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICompanyOwnerService _companyOwnerService;
        private readonly ICompanyService _companyService;
        private readonly IUserHelperService _userHelper;
        private readonly ILogger<CompanyOwnerController> _logger;
        public CompanyOwnerController(
            IAppointmentService appointmentService,
            ICompanyService companyService,
            ICompanyOwnerService companyOwnerService,
            IUserHelperService userHelper,
            ILogger<CompanyOwnerController> logger
            )
        {
            _appointmentService = appointmentService;
            _companyOwnerService = companyOwnerService;
            _userHelper = userHelper;
            _companyService = companyService;
            _logger = logger;
        }

        [HttpPost("appointments")]
        public async Task<IActionResult> CreateAppointments(string companySlug, [FromBody] List<CreateAppointmentDto> appointments)
        {
            try
            {
                ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);
                if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

                Guid userId = userIdResponse.Value;

                bool userOwnsCompany = await _companyOwnerService.UserOwnsCompanyAsync(userId, companySlug);
                if (!userOwnsCompany) return Forbid();

                var company = await _companyService.GetCompanyAsync(companySlug);

                var appts = appointments
                    .Select(a => new Appointment
                    {
                        Company = company!,
                        StartTime = a.StartTime,
                        EndTime = a.EndTime
                    })
                    .ToList();

                var response = _appointmentService.CreateAppointments(appts);
                if (!response.Success) return BadRequest(response.Message);

                return Created();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to create appointments for {companySlug}.");
                return StatusCode(500, ErrorMessages.Generic);
            }
        }
    }
}
