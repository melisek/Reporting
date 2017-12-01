using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using szakdoga;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.RelDtos.RepUserDtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace test_szakdoga
{
    [TestFixture]
    public class ReportUserRelManagerTests
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

            _reports = new List<Report>
            {
                new Report{ Id=1, ReportGUID="Valid", Style="TestStyle"},
                new Report{ Id=2, ReportGUID="InValid", Style="InvalidTestStyle"}
            };

            _reportUserRel = new List<ReportUserRel>
            {
                new ReportUserRel{ Id=1, User=_users[0], Report=_reports[0], AuthoryLayer=(int)ReportUserPermissions.CanModify},
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
        public void Test_Create_Should_CreateRecord_When_ValidDataAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((z) => _reportUserRel.Add(z));
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(z => z == "watch"))).Returns(_users.SingleOrDefault(z => z.UserGUID == "watch"));
            // Act
            ReportUserRelManager manager = new ReportUserRelManager(userRep.Object, reportUserRelRep.Object, reportRep.Object);
            int k = _reportUserRel.Count;
            manager.Create(new CreateReportUserDto
            {
                ReportGUID = "Valid",
                UserGUID = "watch",
                Permission = (int)ReportUserPermissions.CanWatch
            });
            // Assert
            Assert.Greater(_reportUserRel.Count, k);
        }

        [Test]
        public void Test_Create_Should_ThrowBasicException_When_AlreadyContainsValue()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((z) => _reportUserRel.Add(z));
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(z => z == "modify"))).Returns(_users.SingleOrDefault(z => z.UserGUID == "modify"));
            // Act
            ReportUserRelManager manager = new ReportUserRelManager(userRep.Object, reportUserRelRep.Object, reportRep.Object);

            // Assert
            Assert.Throws(typeof(BasicException), () => manager.Create(new CreateReportUserDto
            {
                ReportGUID = "Valid",
                UserGUID = "modify",
                Permission = (int)ReportUserPermissions.CanModify
            }));
        }

        [Test]
        public void Test_Create_Should_ThrowNotFoundException_When_NoUserWithGUID()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((z) => _reportUserRel.Add(z));
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(z => z == "watch"))).Returns(_users.SingleOrDefault(z => z.UserGUID == "watch"));
            // Act
            ReportUserRelManager manager = new ReportUserRelManager(userRep.Object, reportUserRelRep.Object, reportRep.Object);

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.Create(new CreateReportUserDto
            {
                ReportGUID = "Valid",
                UserGUID = "modify123",
                Permission = (int)ReportUserPermissions.CanModify
            }));
        }

        [Test]
        public void Test_Create_Should_ThrowNotFoundException_When_NoReportWithGUID()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "Valid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.Is<string>(y => y == "InValid"))).Returns(_reports.SingleOrDefault(x => x.ReportGUID.Equals("InValid")));
            reportRep.Setup(x => x.GetAll()).Returns(_reports);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 1), It.Is<int>(z => z == 1))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 1 && x.Report.Id == 1));
            reportUserRelRep.Setup(x => x.Get(It.Is<int>(y => y == 2), It.Is<int>(z => z == 2))).Returns(_reportUserRel.SingleOrDefault(x => x.User.Id == 2 && x.Report.Id == 2));
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((z) => _reportUserRel.Add(z));
            Mock<IUserRepository> userRep = new Mock<IUserRepository>();
            userRep.Setup(x => x.Get(It.Is<string>(z => z == "watch"))).Returns(_users.SingleOrDefault(z => z.UserGUID == "watch"));
            // Act
            ReportUserRelManager manager = new ReportUserRelManager(userRep.Object, reportUserRelRep.Object, reportRep.Object);

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.Create(new CreateReportUserDto
            {
                ReportGUID = "Valid123",
                UserGUID = "modify",
                Permission = (int)ReportUserPermissions.CanModify
            }));
        }
    }
}