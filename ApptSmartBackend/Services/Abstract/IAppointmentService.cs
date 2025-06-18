using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IAppointmentService
    {
        public IEnumerable<Appointment> GetAvailableAppointments(string companySlug, DateTime date);
        public IEnumerable<DateTime> GetAvailableDays(string companySlug, int month);
        public Task<GenericResponse<int>> CreateAppointments(string companySlug, List<Appointment> appts); // TODO: Adjust to accept company slug
        public Task<GenericResponse<UserAppointment>> BookAppointment(Guid userId, int apptId);
    }
}
