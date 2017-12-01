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
    public class ReportManagerTests
    {
        private List<User> _users;
        private List<Dashboard> _dashboards;
        private List<UserDashboardRel> _dashUserRel;
        private List<Report> _reports;
        private List<ReportUserRel> _reportUserRel;

        [SetUp]
        public void InitData()
        {
            _users = new List<User>
            {
                new User{Id=1, Deleted=false, Name="CanModify", UserGUID="modify" },
                new User{Id=2, Deleted=false, Name="CanWatch", UserGUID="watch" },
                new User{Id=3, Deleted=false, Name="Invalid", UserGUID="invalid" }
            };
            //_dashboards = new List<Dashboard>
            //{
            //    new Dashboard { Id=1, DashBoardGUID="Valid", Style="StyleTest",Name="Valid", Deleted=false},
            //    new Dashboard { Id=2, DashBoardGUID="InValid", Style="InvalidStyle",Name="InValid", Deleted=false}
            //};
            //_dashUserRel = new List<UserDashboardRel>
            //{
            //    new UserDashboardRel{ User=_users[0], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.CanModify},
            //    new UserDashboardRel{ User=_users[1], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.CanWatch},
            //    new UserDashboardRel{ User=_users[2], Dashboard=_dashboards[0], AuthoryLayer=(int)DashboardUserPermissions.Invalid}
            //};

            _reports = new List<Report>
            {
                new Report{ Id=1, ReportGUID="Valid", Style="TestStyle"},
                new Report{ Id=2, ReportGUID="InValid", Style="InvalidTestStyle"}
            };

            _reportUserRel = new List<ReportUserRel>
            {
                new ReportUserRel{ Id=1, User=_users[0], Report=_reports[0], AuthoryLayer=(int)ReportUserPermissions.CanModify},
                new ReportUserRel{ Id=2, User=_users[1], Report=_reports[0], AuthoryLayer=(int)ReportUserPermissions.CanWatch},
                new ReportUserRel{ Id=3, User=_users[2], Report=_reports[0], AuthoryLayer=(int)ReportUserPermissions.Invalid}
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
        public void Test_GetReportStyle_Should_GiveBackStyle_When_AddedValidReportGUID()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));

            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);
            // Assert
            StringAssert.Contains("TestStyle", manager.GetReportStyle("Valid", _users[0]).Style);
        }

        [Test]
        public void Test_GetReportStyle_Should_ThrowNotFoundException_When_AddedInValidReportGUID()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));

            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);
            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.GetReportStyle("Valid2", _users[0]));
        }

        [Test]
        public void Test_GetReportStyle_Should_ThrowPermissionException_When_AddedUserWithOutWatchPermission()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));

            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);
            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.GetReportStyle("Valid", _users[2]));
        }
    }
}