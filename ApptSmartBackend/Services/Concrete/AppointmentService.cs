using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUserAppointmentRepository _userAppointmentsRepo;
        private readonly IAppointmentRepository _appointmentRepo;
        public AppointmentService(IUserAppointmentRepository userAppointmentsRepo, IAppointmentRepository appointmentRepo)
        {
            _userAppointmentsRepo = userAppointmentsRepo;
            _appointmentRepo = appointmentRepo;
        }

        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId)
        {
            return _userAppointmentsRepo.GetFutureAppointments(userId);
        }

        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId)
        {
            return _userAppointmentsRepo.GetPastAppointments(userId);
        }

        public void CreateAppointments(List<Appointment> appts)
        {
            _appointmentRepo.AddRange(appts);
        }
    }
}
