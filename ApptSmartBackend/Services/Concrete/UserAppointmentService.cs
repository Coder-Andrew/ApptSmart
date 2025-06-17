using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class UserAppointmentService : IUserAppointmentService
    {
        private readonly IUserAppointmentRepository _userAppointmentRepo;
        public UserAppointmentService(IUserAppointmentRepository userAppointmentRepository)
        {
            _userAppointmentRepo = userAppointmentRepository;
        }

        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId)
        {
            return _userAppointmentRepo.GetFutureAppointments(userId);   
        }

        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId)
        {
            return _userAppointmentRepo.GetPastAppointments(userId);
        }
    }
}
