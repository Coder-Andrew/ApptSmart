using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Extensions;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("futureAppointments")]
        public ActionResult<UserAppointmentDto> GetFutureAppointments()
        {
            ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);

            if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;


            var futureAppointments = _appointmentService.GetFutureAppointments(userIdResponse.Value);

            List<UserAppointmentDto> appts = futureAppointments.Select(ui => new UserAppointmentDto
            {
                Id = ui.Id,
                AppointmentTime = ui.DateTime
            }).ToList();

            return Ok(appts);
        }
    }
}
