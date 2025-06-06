using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Extensions;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace ApptSmartBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/appointments")]
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

        // TODO: Fix future and past appointments, needs eager loading, needs proper dtos
        // !!!
        // !!!
        // !!!
        [HttpGet("futureAppointments")]
        public ActionResult<UserAppointmentDto> GetFutureAppointments()
        {
            ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);

            if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;


            var futureAppointments = _appointmentService.GetFutureAppointments(userIdResponse.Value).ToList();

            List<UserAppointmentDto> appts = futureAppointments.Select(ui => ui.ToDto()).ToList();

            return Ok(appts);
        }

        [HttpGet("pastAppointments")]
        public ActionResult<UserAppointmentDto> GetPastAppointments()
        {
            ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);

            if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

            var pastAppointments = _appointmentService.GetPastAppointments(userIdResponse.Value);


            List<UserAppointmentDto> appts = pastAppointments.Select(ui => ui.ToDto()).ToList();

            return Ok(appts);
        }

        [HttpGet("available")]
        public ActionResult<List<AppointmentDto>> GetAvailableAppointments([FromQuery] DateTime date)
        {
            if (date == default)
            {
                
                return BadRequest("Invalid date format");
            }

            return Ok(_appointmentService.GetAvailableAppointments(date)
                .Select(a => a.ToDto())
                .ToList());
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
