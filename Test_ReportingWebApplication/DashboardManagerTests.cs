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
        private List<Report> _reports;
        private List<ReportDashboardRel> _reportDashRel;

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
            _reports = new List<Report>
            {
                new Report{ Id=1, ReportGUID="Valid", Style="TestStyle"},
                new Report{ Id=2, ReportGUID="InValid", Style="InvalidTestStyle"}
            };

            _reportDashRel = new List<ReportDashboardRel>();

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

        [Test]
        public void Test_CreateDashboard_Should_Create_When_ValidDataAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);
            string dashGUID = manager.CreateDashboard(new CreateDashboardDto
            {
                Name = "testing",
                Reports = new List<szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto>
                        {
                                new szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto
                                {  Position="1", ReportGUID="Valid"} }
            },
            _users[0]);

            ReportDashboardRel report = _reportDashRel.FirstOrDefault(x => x.Report.ReportGUID == "Valid" && x.Dashboard.Name == "testing");
            // Assert
            Assert.AreEqual(true, report != null);
        }
        [Test]
        public void Test_UpdateDashboard_Should_Update_When_ValidDataAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);
            bool result = manager.UpdateDashboard(
                new UpdateDashboardDto
                {
                    DashboardGUID = "Valid",
                    Name = "Valid2",
                    Reports = new List<szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto>
                    { new szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto { Position = "1", ReportGUID = "Valid" } }
                }
                , "Valid", _users[0]);

            result = result && _dashboards[0].Name.Equals("Valid2");


            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Test_UpdateDashboard_Should_ThrowNotFoundException_When_InValidDashboardGUIDAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.UpdateDashboard(
                 new UpdateDashboardDto
                 {
                     DashboardGUID = "Valid",
                     Name = "Valid2",
                     Reports = new List<szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto>
                     { new szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto { Position = "1", ReportGUID = "Valid" } }
                 }
                 , "Valid2", _users[0]));
        }
        [Test]
        public void Test_UpdateDashboard_Should_PermissionException_When_UnAuthorizedUserAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);

            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.UpdateDashboard(
                 new UpdateDashboardDto
                 {
                     DashboardGUID = "Valid",
                     Name = "Valid2",
                     Reports = new List<szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto>
                     { new szakdoga.Models.Dtos.QueryDtos.ReportDashboardRelDto { Position = "1", ReportGUID = "Valid" } }
                 }
                 , "Valid", _users[1]));
        }
        [Test]
        public void Test_RemoveDashboard_Should_Remove_When_AuthorizedUserAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            dashboardRep.Setup(x => x.Remove(It.IsAny<string>())).Returns<string>((z) => _dashboards.RemoveAll(x => x.DashBoardGUID == z) > 0);
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);

            // Assert
            Assert.AreEqual(true, manager.DeleteDashboard("Valid", _users[0]));
        }

        [Test]
        public void Test_RemoveDashboard_Should_ThrowNotFoundException_When_InvalidDashboardGUIDAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            dashboardRep.Setup(x => x.Remove(It.IsAny<string>())).Returns<string>((z) => _dashboards.RemoveAll(x => x.DashBoardGUID == z) > 0);
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.DeleteDashboard("Valid2", _users[0]));
        }

        [Test]
        public void Test_RemoveDashboard_Should_ThrowPermissionException_When_UnAuthorizedUserAdded()
        {
            // Arrange
            Mock<IDashboardRepository> dashboardRep = new Mock<IDashboardRepository>();
            dashboardRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((x) => _dashboards.SingleOrDefault(z => z.DashBoardGUID.Equals(x)));
            dashboardRep.Setup(x => x.Add(It.IsAny<Dashboard>())).Callback<Dashboard>((x) => _dashboards.Add(x));
            dashboardRep.Setup(x => x.Update(It.IsAny<Dashboard>())).Callback<Dashboard>((z) => { var dash = _dashboards.Find(x => x.DashBoardGUID == z.DashBoardGUID); dash = z; });
            dashboardRep.Setup(x => x.Remove(It.IsAny<string>())).Returns<string>((z) => _dashboards.RemoveAll(x => x.DashBoardGUID == z) > 0);
            Mock<IUserDashboardRelRepository> userDashRelRep = new Mock<IUserDashboardRelRepository>();
            userDashRelRep.Setup(x => x.Add(It.IsAny<UserDashboardRel>())).Callback<UserDashboardRel>((z) => _dashUserRel.Add(z));
            userDashRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((dashId, userId) => _dashUserRel.SingleOrDefault(z => z.Dashboard.Id == dashId && z.User.Id == userId));
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.Add(It.IsAny<ReportDashboardRel>())).Callback<ReportDashboardRel>(z => _reportDashRel.Add(z));
            repDashRel.Setup(x => x.GetDashboardReports(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Dashboard.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            DashboardManager manager = new DashboardManager(dashboardRep.Object, repDashRel.Object, reportRep.Object, userDashRelRep.Object);

            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.DeleteDashboard("Valid", _users[1]));
        }
    }
}