using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Services.Abstract
{
    public interface IAppointmentService
    {
        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId);
        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId);
        public IEnumerable<Appointment> GetAvailableAppointments(DateTime date);
        public void CreateAppointments(List<Appointment> appts);
        public IEnumerable<DateTime> GetAvailableDays(int month);
        public Task<GenericResponse<UserAppointment>> BookAppointment(Guid userId, int apptId);
    }
}
