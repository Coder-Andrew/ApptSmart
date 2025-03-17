using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Utilities
{
    public class SeedUserInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public List<UserAppointment> Appointments { get; set; }
        
        public SeedUserInfo(string firstName, string lastName, string role, List<UserAppointment>? userAppointments = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Appointments = userAppointments ?? new List<UserAppointment>();
        }
    }

    public class SeedData
    {
        public static readonly SeedUserInfo[] UserSeedData = new SeedUserInfo[]
        {
            new SeedUserInfo("John", "Doe", "User", new List<UserAppointment>
            {
                new UserAppointment
                {   
                    DateTime = DateTime.Now.AddDays(10).AddHours(-10)
                },
                new UserAppointment
                {
                    DateTime = DateTime.Now.AddDays(-5).AddHours(-5)
                }
            }),
            new SeedUserInfo("Sarah", "Doe", "User"),
            new SeedUserInfo("a","a", "Admin", new List<UserAppointment>
            {
                new UserAppointment
                {
                    DateTime = DateTime.Now.AddDays(10).AddHours(5)
                },
                new UserAppointment
                {
                    DateTime = DateTime.Now.AddDays(-5).AddHours(-5)
                },
                new UserAppointment
                {
                    DateTime = DateTime.Now.AddDays(2).AddHours(3)
                }
            })
        };
    }
}
