using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;

namespace ApptSmartBackend.Services.Concrete
{
    public class AppService : IAppService
    {
        private readonly IRepositoryAsync<UserAppointment> _userAppointmentsRepo;
        public AppService(IRepositoryAsync<UserAppointment> userAppointmentsRepo)
        {
            _userAppointmentsRepo = userAppointmentsRepo;
        }

        public Task<GenericResponse<List<UserAppointmentDto>>> GetUserAppointmentsByUserId(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
