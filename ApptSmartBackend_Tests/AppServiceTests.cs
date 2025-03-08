using ApptSmartBackend.DAL.Abstract;
using ApptSmartBackend.DAL.Concrete;
using ApptSmartBackend.Models;
using ApptSmartBackend.Models.AppModels;
using ApptSmartBackend.Services.Abstract;
using ApptSmartBackend.Services.Concrete;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApptSmartBackend_Tests
{
    public class AppServiceTests
    {
        private List<Guid> _guids;
        private List<UserInfo> _users;
        private List<UserAppointment> _userAppointments;
        private Mock<AppDbContext> _contextMock;
        private IRepositoryAsync<UserAppointment> _userRepository;
        private IAppService _appService;

        [SetUp]
        public void Setup()
        {
            _guids = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };

            _userAppointments = new List<UserAppointment>
            {
                new UserAppointment {Id = 1, UserInfoId = guids[0], DateTime = DateTime.Now},
                new UserAppointment {Id = 2, UserInfoId = guids[1], DateTime = DateTime.Now},
                new UserAppointment {Id = 3, UserInfoId = guids[2], DateTime = DateTime.Now},
            };

            _users = new List<UserInfo>
            {
                new UserInfo{Id = guids[0], FirstName = "John", LastName = "Doe"},
                new UserInfo{Id = guids[1], FirstName = "Sally", LastName = "Doe"}
            };

            foreach (var userAppt in _userAppointments)
            {
                userAppt.UserInfo = _users.FirstOrDefault(ui => ui.Id == userAppt.UserInfoId);
            }

            _contextMock = MockDbContextHelper.CreateMockDbContext<AppDbContext>(builder =>
                builder
                .WithDbSet(c => c.UserAppointments, _userAppointments)
                .WithDbSet(c => c.UserInfos, _users)
            );

            _contextMock.Setup(c => c.SaveChanges()).Returns(1);
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            _userRepository = new RepositoryAsync<UserAppointment>(_contextMock.Object);

            _appService = new AppService(_userRepository);
        }

        [Test]
        public async Task GetAppointmentsById_WithValidId_ReturnsUserAppointments()
        {
            // Arrange

            // Act
            // Assert
        }
    }
}
