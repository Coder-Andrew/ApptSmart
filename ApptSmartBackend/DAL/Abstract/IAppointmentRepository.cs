using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        public IEnumerable<Appointment> GetAvailableAppointments(DateTime date);
    }
}
