using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Utilities
{
    public class SeedUserInfo
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
        public List<UserAppointment> Appointments { get; set; } = new();
    }
}