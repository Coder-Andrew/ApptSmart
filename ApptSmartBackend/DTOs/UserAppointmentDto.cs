namespace ApptSmartBackend.DTOs
{
    public class UserAppointmentDto
    {
        public int Id { get; set; }
        public DateTime? AppointmentTime { get; set; }
    }
    public class UserAppointmentsDto
    {
        public List<UserAppointmentDto> FutureAppointments { get; set; } = new();
        public List<UserAppointmentDto> PastAppointments { get; set; } = new();
    }
}
