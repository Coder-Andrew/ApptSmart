//using ApptSmartBackend.DAL.Abstract;
//using ApptSmartBackend.Models;
//using ApptSmartBackend.Models.AppModels;
//using Microsoft.EntityFrameworkCore;
//using Moq;
//using System;

//namespace ApptSmartBackend_Tests;

//public class UserRepositoryTests
//{
//    private List<UserInfo> _users;
//    private List<UserAppointment> _userAppointments;
//    private Mock<AppDbContext> _contextMock;
//    private UserRepository _userRepository

//    [SetUp]
//    public void Setup()
//    {
//        _userAppointments = new List<UserAppointment>
//        {
//            new UserAppointment {Id = 1, UserInfoId = new Guid("a"), DateTime = DateTime.Now},
//            new UserAppointment {Id = 2, UserInfoId = new Guid("b"), DateTime = DateTime.Now},
//            new UserAppointment {Id = 3, UserInfoId = new Guid("c"), DateTime = DateTime.Now},
//        };

//        _users = new List<UserInfo>
//        {
//            new UserInfo{Id = new Guid("a"), FirstName = "John", LastName = "Doe"},
//            new UserInfo{Id = new Guid("b"), FirstName = "Sally", LastName = "Doe"}
//        };

//        foreach (var userAppt in _userAppointments)
//        {
//            userAppt.UserInfo = _users.FirstOrDefault(ui => ui.Id == userAppt.UserInfoId);
//        }

//        _contextMock = MockDbContextHelper.CreateMockDbContext<AppDbContext>(builder =>
//            builder
//            .WithDbSet(c => c.UserAppointments, _userAppointments)
//            .WithDbSet(c => c.UserInfos, _users)         
//        );

//        _contextMock.Setup(c => c.SaveChanges()).Returns(1);
//        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


//        _userRepository = new UserRepository(_contextMock.Object);
//    }

//    [Test]
//    public void Test1()
//    {
//        Assert.Pass();
//    }
//}
