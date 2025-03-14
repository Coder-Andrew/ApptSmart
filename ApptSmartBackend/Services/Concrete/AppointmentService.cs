using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUserAppointmentRepository _userAppointmentsRepo;
        public AppointmentService(IUserAppointmentRepository userAppointmentsRepo)
        {
            _userAppointmentsRepo = userAppointmentsRepo;
        }

        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId)
        {
            return _userAppointmentsRepo.GetFutureAppointments(userId);
        }

        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId)
        {
            return _userAppointmentsRepo.GetPastAppointments(userId);
        }
    }
}
