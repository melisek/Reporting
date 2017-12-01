using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using szakdoga;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.ReportDtos;
using szakdoga.Others;

namespace test_szakdoga
{
    [TestFixture]
    public class DashboardManagerTests
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
        public void Test_GetDashBoardStyle_Should_GiveBackRigthStyle_When_AddedValidGUID()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("Valid")));
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("InValid")));
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_dashUserRel.SingleOrDefault(x => x.Dashboard.Id == 1 && x.User.Id == 1));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, null, null, userDashRelRep.Object);
            var testStyle = manager.GetDashBoardStyle("Valid", _users[0]).Style;
            // Assert
            StringAssert.Contains("StyleTest", testStyle);
        }

        [Test]
        public void Test_GetDashBoardStyle_ShouldThrowPermissionException_When_NoRigthToWatch()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("Valid")));
            dashboardRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_dashboards.SingleOrDefault(x => x.DashBoardGUID.Equals("InValid")));
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_dashUserRel.SingleOrDefault(x => x.Dashboard.Id == 1 && x.User.Id == 1));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, null, null, userDashRelRep.Object);

            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.GetDashBoardStyle("Valid", _users[2]));
        }
    }
}