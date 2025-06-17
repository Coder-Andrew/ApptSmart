using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Extensions;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ApptSmartBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/{companySlug}/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserInfoRepositoryAsync _userInfoRepositoryAsync;
        private readonly IUserHelperService _userHelper;
        public AppointmentController(IAppointmentService appointmentService, IUserInfoRepositoryAsync userInfoRepository, IUserHelperService userHelper)
        {
            _appointmentService = appointmentService;
            _userInfoRepositoryAsync = userInfoRepository;
            _userHelper = userHelper;
        }

        [HttpGet("available")]
        public ActionResult<List<AppointmentDto>> GetAvailableAppointments(string companySlug, [FromQuery] DateTime date)
        {
            if (date == default)
            {                
                return BadRequest("Invalid date format");
            }

            var appts = _appointmentService.GetAvailableAppointments(companySlug, date)
                .Select(a => a.ToDto())
                .ToList();

            return Ok(appts);
        }

        [HttpGet("available/{month:int}")]
        public ActionResult<List<DateTime>> GetDaysWithAvailableDays(string companySlug, int month)
        {
            if (month < 0 || month > 12)
            {
                return BadRequest("Invalid month");
            }

            var appts = _appointmentService.GetAvailableDays(companySlug, month).ToList();

            return Ok(appts);
        }


        [HttpPost("book")]
        public async Task<ActionResult> BookAppointment([FromBody] BookAppointmentDto bookAppointment)
        {
            try
            {
                ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);

                if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

                var response = await _appointmentService.BookAppointment(userIdResponse.Value, bookAppointment.AppointmentId);

                if (!response.Success)
                {
                    return BadRequest(response.Message);
                }
                               
                return Ok(response.Data?.Id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create")]
        public ActionResult CreateAppointments([FromBody] List<CreateAppointmentDto> appointments)
        {
            try
            {
                List<Appointment> appts = appointments.Select(a => new Appointment
                {
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList();

                _appointmentService.CreateAppointments(appts);

                return Created();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
