using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface IUserAppointmentRepository : IRepositoryAsync<UserAppointment>
    {
        IEnumerable<UserAppointment> GetFutureAppointments(Guid userId);
        IEnumerable<UserAppointment> GetPastAppointments(Guid userId);
    }
}
