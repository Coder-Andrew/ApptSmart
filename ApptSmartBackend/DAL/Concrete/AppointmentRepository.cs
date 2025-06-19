using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    /// <summary>
    /// An implementation of the Appointment table using the repository pattern
    /// </summary>
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private DbSet<Appointment> _appointments;
        public AppointmentRepository(AppDbContext ctx) : base(ctx)
        {
            _appointments = ctx.Appointments;
        }

        // Maybe use better string comparison methods, like converting to lower...
        /// <summary>
        /// Retrieves all unbooked appointments for a specific company on a given date.
        /// </summary>
        /// <param name="companySlug">The unique slug identifier of the company</param>
        /// <param name="date">The specific day to check for availability.</param>
        /// <returns>A list of appointments where no user has booked (UserAppointment is null) on the specified date, sorted by start time.</returns>
        public IEnumerable<Appointment> GetAvailableAppointments(string companySlug, DateTime date)
        {
            return _appointments
                //.Include(a => a.Company) // Don't need right now, maybe remove until needed
                .Where(a => a.Company.CompanySlug == companySlug &&
                    a.StartTime.Date == date.Date &&
                    a.UserAppointment == null)
                .OrderBy(a => a.StartTime)
                .ToList();
        }

        /// <summary>
        /// Retrieves a list of unique dates within the specified month that have at least one unbooked appointment for the given company.
        /// </summary>
        /// <param name="companySlug">The unique slug identifier of the company</param>
        /// <param name="month">The numeric month (1-12) to search for availability.</param>
        /// <returns>A list of unique dates with available appointments</returns>
        public IEnumerable<DateTime> GetAvailableDays(string companySlug, int month)
        {
            return _appointments
                //.Include(a => a.Company) // Don't need right now, maybe remove until needed
                .Where(a => a.Company.CompanySlug == companySlug &&
                    a.UserAppointment == null &&
                    a.StartTime.Month == month)
                .GroupBy(a => a.StartTime.Date)
                .Select(a => a.Key)
                .OrderBy(date => date)
                .ToList();
        }

        public override Appointment? FindById(int id)
        {
            return _appointments
                .Include(a => a.UserAppointment)
                .Include(a => a.Company)
                .FirstOrDefault(a => a.Id == id);
        }
    }
}
