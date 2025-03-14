using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IAppointmentService
    {
        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId);
        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId);
    }
}
