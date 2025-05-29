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
                    BookedAt = DateTime.Now.AddDays(-10),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(-5),
                        EndTime = DateTime.Now.AddDays(-5).AddMinutes(30)
                    }
                },
                new UserAppointment
                {
                    BookedAt = DateTime.Now.AddDays(5),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(10),
                        EndTime = DateTime.Now.AddDays(10).AddHours(2)
                    }
                }
            }),
            new SeedUserInfo("Sarah", "Doe", "User"),
            new SeedUserInfo("a","a", "Admin", new List<UserAppointment>
            {
                new UserAppointment
                {
                    BookedAt = DateTime.Now.AddDays(10).AddHours(5),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(11).AddHours(5),
                        EndTime = DateTime.Now.AddDays(12),
                    }
                },
                new UserAppointment
                {
                    BookedAt = DateTime.Now.AddDays(-5).AddHours(-5),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(-5).AddHours(-1),
                        EndTime = DateTime.Now.AddDays(-5)
                    }
                },
                new UserAppointment
                {
                    BookedAt = DateTime.Now,
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(60).AddHours(2),
                        EndTime = DateTime.Now.AddDays(61)
                    }
                }
            })
        };
    }
}
