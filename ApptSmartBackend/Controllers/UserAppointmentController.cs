using ApptSmartBackend.DTOs;
using ApptSmartBackend.Extensions;
using ApptSmartBackend.Services;
using ApptSmartBackend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApptSmartBackend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/userAppointments")]
    public class UserAppointmentController : ControllerBase
    {
        private readonly IUserAppointmentService _userAppointmentService;
        private readonly IUserHelperService _userHelper;
        public UserAppointmentController(IUserHelperService userHelper, IUserAppointmentService userAppointmentService)
        {
            _userHelper = userHelper;
            _userAppointmentService = userAppointmentService;
        }

        [HttpGet("futureAppointments")]
        public ActionResult<IEnumerable<UserAppointmentDto>> GetFutureAppointments()
        {
            ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);
            if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

            Guid userId = userIdResponse.Value;

            var appts = _userAppointmentService.GetFutureAppointments(userId)
                .Select(ua => ua.ToDto())
                .ToList();

            return Ok(appts);
        }

        [HttpGet("pastAppointments")]
        public ActionResult<IEnumerable<UserAppointmentDto>> GetPastAppointments()
        {
            ActionResult<Guid> userIdResponse = this.GetUserId(_userHelper);
            if (userIdResponse.Result is UnauthorizedResult || userIdResponse.Result is NotFoundResult) return userIdResponse.Result;

            Guid userId = userIdResponse.Value;

            var appts = _userAppointmentService.GetPastAppointments(userId)
                .Select(ua => ua.ToDto())
                .ToList();

            return Ok(appts);
        }
    }
}
