using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    // TODO: Find out how to use async
    public class UserAppointmentRepository : RepositoryAsync<UserAppointment>, IUserAppointmentRepository
    {
        private DbSet<UserAppointment> _userAppointments;
        public UserAppointmentRepository(AppDbContext context) : base(context)
        {
            _userAppointments = context.UserAppointments;
        }

        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId)
        {
            return _userAppointments
                .Where(ua => ua.UserInfoId == userId && ua.Appointment.StartTime >= DateTime.Now);
        }

        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId)
        {
            return _userAppointments
                .Where(ua => ua.UserInfoId == userId && ua.Appointment.StartTime < DateTime.Now);
        }
    }
}
