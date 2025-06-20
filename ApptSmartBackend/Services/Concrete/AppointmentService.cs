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
        private readonly ICompanyRepository _companyRepository;
        public AppointmentService(IUserAppointmentRepository userAppointmentsRepo, IAppointmentRepository appointmentRepo, ICompanyRepository companyRepository)
        {
            _userAppointmentsRepo = userAppointmentsRepo;
            _appointmentRepo = appointmentRepo;
            _companyRepository = companyRepository;
        }

        public GenericResponse<int> CreateAppointments(List<Appointment> appts)
        {
            _appointmentRepo.AddRange(appts); 
            return new GenericResponse<int>
            {
                Data = appts.Count,
                Success = true,
                StatusCode = GenericStatusCode.AppointmentsCreated
            };
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
            if (appt.StartTime < DateTime.UtcNow)
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
                BookedAt = DateTime.UtcNow,
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
