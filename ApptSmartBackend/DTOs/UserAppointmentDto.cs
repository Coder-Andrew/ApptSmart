using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.DTOs
{
    public class UserAppointmentDto
    {
        public int Id { get; set; }
        public AppointmentDto Appointment { get; set; } = new();
        public DateTime? BookedAt { get; set; }
    }

}
