using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;
using Microsoft.Identity.Client;

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

        public IEnumerable<Appointment> GetAvailableAppointments(DateTime date)
        {
            return _appointmentRepo.GetAvailableAppointments(date);
        }

        public async Task<UserAppointment> BookAppointment(Guid userId, int apptId)
        {
            // TODO: Add error handling/checking to make sure appt isn't already taken/exists          
            var appt = _appointmentRepo.FindById(apptId);
            if (appt.UserAppointment != null)
            {
                throw new Exception($"Appointment {apptId} already booked!");
            }
            var userAppt = new UserAppointment
            {
                UserInfoId = userId,
                Appointment = appt,
                BookedAt = DateTime.Now,
            };
            await _userAppointmentsRepo.AddOrUpdateAsync(userAppt);
            return userAppt;
        }

        public IEnumerable<DateTime> GetAvailableDays(int month)
        {
            return _appointmentRepo.GetAvailableDays(month).ToList();
        }
    }
}
