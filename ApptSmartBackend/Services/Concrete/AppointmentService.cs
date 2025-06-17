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

        public void CreateAppointments(List<Appointment> appts)
        {
            _appointmentRepo.AddRange(appts);
        }

        public IEnumerable<Appointment> GetAvailableAppointments(string companySlug, DateTime date)
        {
            return _appointmentRepo.GetAvailableAppointments(companySlug, date);
        }

        public async Task<GenericResponse<UserAppointment>> BookAppointment(Guid userId, int apptId)
        { 
            var appt = _appointmentRepo.FindById(apptId);
            if (appt == null)
            {
                return new GenericResponse<UserAppointment>
                {
                    Data = default,
                    Message = "Appointment not found",
                    Success = false,
                    StatusCode = GenericStatusCode.AppointmentNotFound
                };
            }
            if (appt.UserAppointment != null)
            {
                return new GenericResponse<UserAppointment>
                {
                    Data = default,
                    Message = "Appointment already booked",
                    Success = false,
                    StatusCode = GenericStatusCode.AppointmentAlreadyBooked
                };
            }
            if (appt.StartTime < DateTime.Now)
            {
                return new GenericResponse<UserAppointment>
                {
                    Data = default,
                    Message = "Unable to book past appointments",
                    Success = false,
                    StatusCode = GenericStatusCode.BookPastAppointment
                };
            }

            var userAppt = new UserAppointment
            {
                UserInfoId = userId,
                Appointment = appt,
                BookedAt = DateTime.Now,
            };
            await _userAppointmentsRepo.AddOrUpdateAsync(userAppt);
            return new GenericResponse<UserAppointment>
            {
                Data = userAppt,
                Success = true,
                Message = $"Booked appointment {appt.Id}, under userappt {userAppt.Id}",
                StatusCode = GenericStatusCode.AppointmentBooked
            };
        }

        public IEnumerable<DateTime> GetAvailableDays(string companySlug, int month)
        {
            return _appointmentRepo.GetAvailableDays(companySlug, month);
        }
    }
}
