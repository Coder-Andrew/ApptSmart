using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    // TODO: Consider implementing async versions of GetFutureAppointments and GetPastAppointments to improve scalability.
    /// <summary>
    /// Provides data access methods for UserAppointment entities using the repository pattern.
    /// </summary>
    public class UserAppointmentRepository : RepositoryAsync<UserAppointment>, IUserAppointmentRepository
    {
        private DbSet<UserAppointment> _userAppointments;
        public UserAppointmentRepository(AppDbContext context) : base(context)
        {
            _userAppointments = context.UserAppointments;
        }

        /// <summary>
        /// Retrieves all future appointments booked by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose upcoming appointments are being retrieved.</param>
        /// <returns>A collection of UserAppointment entities where the associated Appointment start time is in the future. Includes the related Appointment and Company entities</returns>
        public IEnumerable<UserAppointment> GetFutureAppointments(Guid userId)
        {
            return _userAppointments
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Company)
                .Where(ua => ua.UserInfoId == userId && ua.Appointment.StartTime >= DateTime.UtcNow);
        }

        /// <summary>
        /// Retrieves all past appointments booked by a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose past appointments are being retrieved.</param>
        /// <returns>A collection of UserAppointment entities where the associated Appointment start time is in the past. Includes the related Appointment and Company entities.</returns>
        public IEnumerable<UserAppointment> GetPastAppointments(Guid userId)
        {
            return _userAppointments
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Company)
                .Where(ua => ua.UserInfoId == userId && ua.Appointment.StartTime < DateTime.UtcNow);
        }
    }
}
