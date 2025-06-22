using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DAL.Abstract
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        public IEnumerable<Appointment> GetAvailableAppointments(string companySlug, DateTime date);
        IEnumerable<DateTime> GetAvailableDays(string companySlug, int month);
    }
}
