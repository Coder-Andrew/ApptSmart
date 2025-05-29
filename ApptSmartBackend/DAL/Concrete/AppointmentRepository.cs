using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;

namespace ApptSmartBackend.DAL.Concrete
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private DbSet<Appointment> _appointments;
        public AppointmentRepository(AppDbContext ctx) : base(ctx)
        {
            _appointments = ctx.Appointments;
        }
    }
}
