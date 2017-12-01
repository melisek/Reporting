using AutoMapper;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using szakdoga;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace test_szakdoga
{
    [TestFixture]
    public class DashboardUserRelManagerTests
    {
        private List<User> _users;
        private List<Dashboard> _dashboards;
        private List<UserDashboardRel> _dashUserRel;

        [SetUp]
        public void InitData()
        {
            _users = new List<User>
            {
                new User{Id=1, Deleted=false, Name="CanModify", UserGUID="modify" },
                new User{Id=2, Deleted=false, Name="CanWatch", UserGUID="watch" },
                new User{Id=3, Deleted=false, Name="Invalid", UserGUID="invalid" }
            };
            _dashboards = new List<Dashboard>
            {
                new Dashboard { Id=1, DashBoardGUID="Valid", Style="StyleTest",Name="Valid", Deleted=false},
                new Dashboard { Id=2, DashBoardGUID="InValid", Style="InvalidStyle",Name="InValid", Deleted=false}
            };
            _dashUserRel = new List<UserDashboardRel>
            {
                new UserDashboardRel{ User=_users[0], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.CanModify},
                new UserDashboardRel{ User=_users[1], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.CanWatch},
                new UserDashboardRel{ User=_users[2], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.Invalid}
            };

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Dashboard, DashboardDto>();
                cfg.CreateMap<Dashboard, DashboardForAllDto>();
                cfg.CreateMap<Report, ReportDto>();
                cfg.CreateMap<Report, ReportForAllDto>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<Query, QueryDto>();
                cfg.CreateMap<User, DashboardUserDto>();
            });
        }

        [TearDown]
        public void Dispose()
        {
            _users = null;
            _dashboards = null;
            _dashUserRel = null;
            AutoMapper.Mapper.Reset();
        }

        [Test]
        public void Test_GetDashboardUsers_Should_GiveBackDashboardUsers_When_ValidGUIDAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("Valid")));
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("InValid")));
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.GetDashboardUsers(It.Is<int>(y => y == 1))).Returns(_dashUserRel.Where(z => z.Dashboard.Id == 1).ToList());
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "CanModify"))).Returns(_users[0]);
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "CanWatch"))).Returns(_users[1]);
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "Invalid"))).Returns(_users[2]);

            // Act
            DashboardUserRelManager manager = new DashboardUserRelManager(userRep.Object, dashboardRep.Object, userDashRelRep.Object);
            var refDto = new DashboardUsersDto { DashboardGUID = "Valid", Users = Mapper.Map<IEnumerable<DashboardUserDto>>(_users) };

            // Assert
            Assert.AreEqual(refDto, manager.GetDashboardUsers("Valid"));
        }

        [Test]
        public void Test_GetDashboardUsers_Should_ThrowNotFoundException_When_InValidGUIDAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("Valid")));
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("InValid")));
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.GetDashboardUsers(It.Is<int>(y => y == 1))).Returns(_dashUserRel.Where(z => z.Dashboard.Id == 1).ToList());
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "CanModify"))).Returns(_users[0]);
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "CanWatch"))).Returns(_users[1]);
            userRep.Setup(x => x.Get(It.Is<string>(y => y == "Invalid"))).Returns(_users[2]);

            // Act
            DashboardUserRelManager manager = new DashboardUserRelManager(userRep.Object, dashboardRep.Object, userDashRelRep.Object);
            var refDto = new DashboardUsersDto { DashboardGUID = "Valid", Users = Mapper.Map<IEnumerable<DashboardUserDto>>(_users) };

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.GetDashboardUsers("InValid1"));
        }
    }
}