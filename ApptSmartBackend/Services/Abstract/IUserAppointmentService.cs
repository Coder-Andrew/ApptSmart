using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IUserAppointmentService
    {
        IEnumerable<UserAppointment> GetFutureAppointments(Guid userId);
        IEnumerable<UserAppointment> GetPastAppointments(Guid userId);
    }
}
