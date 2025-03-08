using ApptSmartBackend.DTOs;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IAppService
    {
        public Task<GenericResponse<List<UserAppointmentDto>>> GetUserAppointmentsByUserId(string userId);
    }
}
