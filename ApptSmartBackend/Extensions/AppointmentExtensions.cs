using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Extensions
{
    public static class AppointmentExtensions
    {
        public static AppointmentDto ToDto(this Appointment model)
        {
            return new AppointmentDto
            {
                Id = model.Id,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
        }
        public static Appointment ToModel(this AppointmentDto model)
        {
            return new Appointment
            {
                Id = model.Id,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };
        }
    }
}
