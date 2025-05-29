using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Moq;
using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DAL.Concrete;
using ApptSmartBackend.Services.Concrete;


namespace ApptSmartBackend_Tests;


public class UserAppointmentServiceTests : TestDbHelper
{
    private static readonly Dictionary<(string, string), Guid> userInfo = new()
    {
        {("John", "Doe"), Guid.NewGuid()},
        {("Sally", "Doe"), Guid.NewGuid()}
    };

    protected override void SeedDatabase(AppDbContext context)
    {
        foreach (var kvp in userInfo)
        {
            context.UserInfos.Add(new UserInfo { Id = kvp.Value, AspNetIdentityId = Guid.NewGuid().ToString(), FirstName = kvp.Key.Item1, LastName = kvp.Key.Item2 });
        }


        List<Appointment> appts = new()
        {
            new Appointment { Id=1, StartTime = DateTime.Now.AddDays(5), EndTime = DateTime.Now.AddDays(5).AddMinutes(60) },
            new Appointment { Id=2, StartTime = DateTime.Now.AddDays(7), EndTime = DateTime.Now.AddDays(7).AddMinutes(30) },
            new Appointment { Id=3, StartTime = DateTime.Now.AddDays(-2), EndTime = DateTime.Now.AddDays(-2).AddMinutes(20) },
            new Appointment { Id=4, StartTime = DateTime.Now.AddDays(-3), EndTime = DateTime.Now.AddDays(-3).AddMinutes(65) },
            new Appointment { Id=5, StartTime = DateTime.Now.AddDays(10), EndTime = DateTime.Now.AddDays(10).AddMinutes(120) },

            new Appointment { Id=6, StartTime = DateTime.Now.AddDays(-2), EndTime = DateTime.Now.AddDays(-2).AddHours(2) },
            new Appointment { Id=7, StartTime = DateTime.Now.AddDays(-3), EndTime = DateTime.Now.AddDays(-3).AddHours(6) }
        };


        context.Appointments.AddRange(appts);

        context.UserAppointments.AddRange(
            new UserAppointment { Id = 1, UserInfoId = userInfo[("John", "Doe")], Appointment = appts[0], BookedAt = DateTime.Now.AddDays(5).AddMinutes(60) },
            new UserAppointment { Id = 2, UserInfoId = userInfo[("John", "Doe")], Appointment = appts[1], BookedAt = DateTime.Now.AddDays(7).AddMinutes(30) },
            new UserAppointment { Id = 3, UserInfoId = userInfo[("John", "Doe")], Appointment = appts[2], BookedAt = DateTime.Now.AddDays(-2).AddMinutes(20) },
            new UserAppointment { Id = 4, UserInfoId = userInfo[("John", "Doe")], Appointment = appts[3], BookedAt = DateTime.Now.AddDays(-3).AddMinutes(65) },
            new UserAppointment { Id = 5, UserInfoId = userInfo[("John", "Doe")], Appointment = appts[4], BookedAt = DateTime.Now.AddDays(10).AddMinutes(120) },

            new UserAppointment { Id = 6, UserInfoId = userInfo[("Sally", "Doe")], Appointment = appts[5], BookedAt = DateTime.Now.AddDays(-2).AddMinutes(2) },
            new UserAppointment { Id = 7, UserInfoId = userInfo[("Sally", "Doe")], Appointment = appts[6], BookedAt = DateTime.Now.AddDays(-3).AddHours(6) }
        );

        context.SaveChanges();
    }

    public static IEnumerable<TestCaseData> FutureUserAppointmentCases
    {
        get
        {
            yield return new TestCaseData(userInfo[("John", "Doe")], new List<int> { 1, 2, 5 }).SetName("John_Doe_Future_Appts");
            yield return new TestCaseData(userInfo[("Sally", "Doe")], new List<int> { }).SetName("Sally_Doe_Future_Appts");
        }
    }

    public static IEnumerable<TestCaseData> PastUserAppointmentCases
    {
        get
        {
            yield return new TestCaseData(userInfo[("John", "Doe")], new List<int> { 3, 4 }).SetName("John_Doe_Past_Appts");
            yield return new TestCaseData(userInfo[("Sally", "Doe")], new List<int> { 6, 7 }).SetName("Sally_Doe_Past_Appts");
        }
    }



    [TestCaseSource(nameof(FutureUserAppointmentCases))]
    public async Task GetFutureAppointmentsById_WithValidId_ReturnsUserAppointments(Guid userId, List<int> expectedAppointmentIds)
    {
        // Arrange
        using AppDbContext context = CreateContext();
        var userRepo = new UserAppointmentRepository(context);
        var appService = new AppointmentService(userRepo);

        // Act
        var futureAppointments = appService.GetFutureAppointments(userId);
    
        // Assert
        Assert.That(futureAppointments.Count() == expectedAppointmentIds.Count());

        foreach (int apptId in futureAppointments.Select(a => a.Id).ToList())
        {
            Assert.That(expectedAppointmentIds.Contains(apptId));
        }
    }

    [TestCaseSource(nameof(PastUserAppointmentCases))]
    public async Task GetPastAppointmentsById_WithValidId_ReturnsUserAppointments(Guid userId, List<int> expectedAppointmentIds)
    {
        // Arrange
        using AppDbContext context = CreateContext();
        var userRepo = new UserAppointmentRepository(context);
        var appService = new AppointmentService(userRepo);

        // Act
        var futureAppointments = appService.GetPastAppointments(userId);

        // Assert
        Assert.That(futureAppointments.Count() == expectedAppointmentIds.Count());

        foreach (int apptId in futureAppointments.Select(a => a.Id).ToList())
        {
            Assert.That(expectedAppointmentIds.Contains(apptId));
        }
    }


}
