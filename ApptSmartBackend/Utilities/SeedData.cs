using ApptSmartBackend.Models.AppModels;

namespace ApptSmartBackend.Utilities;
public static class SeedData
{
    public static readonly List<SeedCompany> Companies = new List<SeedCompany>
    {
        new SeedCompany(
            "ApptSmart",
            "The default ApptSmart booking system! Schedule/book and appointment to meet with an ApptSmart representative!",
            new SeedUserInfo
            {
                FirstName = "a",
                LastName = "a",
                Roles = ["Admin", "BusinessOwner"],
            },
            new List<Appointment>
            {
                new Appointment
                {
                    StartTime = DateTime.Now.AddDays(3),
                    EndTime = DateTime.Now.AddDays(3).AddHours(1),
                },
                new Appointment
                {
                    StartTime = DateTime.Now.AddDays(4),
                    EndTime = DateTime.Now.AddDays(4).AddHours(1),
                },
                new Appointment
                {
                    StartTime = DateTime.Now.AddDays(-5),
                    EndTime = DateTime.Now.AddDays(-5).AddHours(2),
                },
                new Appointment
                {
                    StartTime = DateTime.Now.AddDays(5),
                    EndTime = DateTime.Now.AddDays(5).AddHours(3)
                }
            }
        ),
    };
    public static readonly SeedUserInfo[] UserSeedData = new SeedUserInfo[]
    {
        new SeedUserInfo
        {
            FirstName = "a",
            LastName = "a",
            Roles = ["Admin", "CompanyOwner"],
            Appointments = new List<UserAppointment>
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
            }
        },
        new SeedUserInfo
        {
            FirstName = "John",
            LastName = "Doe",
            Roles = ["User"],
            Appointments = new List<UserAppointment>
            {
                new UserAppointment()
                {
                    BookedAt = DateTime.Now.AddDays(-10),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(-5),
                        EndTime = DateTime.Now.AddDays(-5).AddMinutes(30)
                    }
                },
                new UserAppointment()
                {
                    BookedAt = DateTime.Now.AddDays(5),
                    Appointment = new Appointment
                    {
                        StartTime = DateTime.Now.AddDays(10),
                        EndTime = DateTime.Now.AddDays(10).AddHours(2)
                    }
                }
            },
        },
        new SeedUserInfo
        {
            FirstName = "Sarah",
            LastName = "Doe",
            Roles = ["User"],
        },
    };
}