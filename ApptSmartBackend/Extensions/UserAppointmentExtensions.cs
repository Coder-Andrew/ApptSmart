using ApptSmartBackend.DTOs;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Extensions
{
    public static class UserAppointmentExtensions
    {
        public static UserAppointmentDto ToDto(this UserAppointment model)
        {
            return new UserAppointmentDto
            {
                Id = model.Id,
                Appointment = model.Appointment.ToDto(),
                BookedAt = model.BookedAt
            };
        }

        public static UserAppointment ToModel(this UserAppointmentDto dto)
        {
            return new UserAppointment
            {
                Id = dto.Id,
                Appointment = dto.Appointment.ToModel(),
                BookedAt = dto.BookedAt
            };
        }
    }
}
