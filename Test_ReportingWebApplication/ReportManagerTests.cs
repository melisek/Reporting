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
        private List<Query> _queries;
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

            _queries = new List<Query>
            {
                new Query{  Id=1, Name="Test_Query",QueryGUID="Valid", SQL="sql" }
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
            _queries = null;
            _reportDashRel = null;
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

        [Test]
        public void Test_CreateReport_Should_Create_When_ValidDataAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);
            manager.CreateReport(new CreateReportDto { QueryGUID = "Valid", Columns = new string[] { "1", "2" }, Filter = string.Empty, Name = "Test", Rows = 2, Sort = new SortDto { ColumnName = "Voucher", Direction = Direction.Asc } }, _users[0]);

            // Assert
            Assert.AreEqual(true, _reports.FirstOrDefault(x => x.Name == "Test") != null);
        }

        [Test]
        public void Test_UpdateReport_Should_Update_When_ValidDataAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            reportRep.Setup(x => x.Update(It.IsAny<Report>())).Callback<Report>((z) => { var origrep = _reports.SingleOrDefault(x => x.ReportGUID.Equals(z.ReportGUID)); origrep = z; });
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            reportUserRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((repId, userId) => _reportUserRel.SingleOrDefault(x => x.User.Id == userId && x.Report.Id == repId));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);
            bool result = manager.UpdateReport(new UpdateReportDto { ReportGUID = "Valid", QueryGUID = "Valid", Columns = new string[] { "1", "2" }, Filter = string.Empty, Name = "Test", Rows = 2, Sort = new SortDto { ColumnName = "Voucher", Direction = Direction.Asc } }, "Valid", _users[0]);
            result = result && _reports[0].Name.Equals("Test");
            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Test_UpdateReport_Should_ThrowNotFoundException_When_InValidReportGUIDAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            reportRep.Setup(x => x.Update(It.IsAny<Report>())).Callback<Report>((z) => { var origrep = _reports.SingleOrDefault(x => x.ReportGUID.Equals(z.ReportGUID)); origrep = z; });
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            reportUserRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((repId, userId) => _reportUserRel.SingleOrDefault(x => x.User.Id == userId && x.Report.Id == repId));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);

            // Assert
            Assert.Throws(typeof(NotFoundException), () => manager.UpdateReport(new UpdateReportDto { ReportGUID = "Valid", QueryGUID = "Valid", Columns = new string[] { "1", "2" }, Filter = string.Empty, Name = "Test", Rows = 2, Sort = new SortDto { ColumnName = "Voucher", Direction = Direction.Asc } }, "Valid1", _users[0]));
        }

        [Test]
        public void Test_UpdateReport_Should_ThrowPermissionException_When_UnAuthorizedUserAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            reportRep.Setup(x => x.Update(It.IsAny<Report>())).Callback<Report>((z) => { var origrep = _reports.SingleOrDefault(x => x.ReportGUID.Equals(z.ReportGUID)); origrep = z; });
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            reportUserRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((repId, userId) => _reportUserRel.SingleOrDefault(x => x.User.Id == userId && x.Report.Id == repId));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, null, null, null, null, reportUserRelRep.Object);

            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.UpdateReport(new UpdateReportDto { ReportGUID = "Valid", QueryGUID = "Valid", Columns = new string[] { "1", "2" }, Filter = string.Empty, Name = "Test", Rows = 2, Sort = new SortDto { ColumnName = "Voucher", Direction = Direction.Asc } }, "Valid", _users[1]));
        }

        [Test]
        public void Test_DeleteReport_Should_Delete_When_ValidDataAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            reportRep.Setup(x => x.Update(It.IsAny<Report>())).Callback<Report>((z) => { var origrep = _reports.SingleOrDefault(x => x.ReportGUID.Equals(z.ReportGUID)); origrep = z; });
            reportRep.Setup(x => x.Remove(It.IsAny<string>())).Returns<string>((z) => _reports.RemoveAll(x => x.ReportGUID == z) > 0);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            reportUserRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((repId, userId) => _reportUserRel.SingleOrDefault(x => x.User.Id == userId && x.Report.Id == repId));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.GetReportDashboards(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Report.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, repDashRel.Object, null, null, null, reportUserRelRep.Object);

            // Assert
            Assert.AreEqual(true, manager.DeleteReport("Valid", _users[0]));
        }

        [Test]
        public void Test_DeleteReport_Should_PermissionException_When_UnAuthorizedUserAdded()
        {
            // Arrange
            Mock<IReportRepository> reportRep = new Mock<IReportRepository>();
            reportRep.Setup(x => x.Add(It.IsAny<Report>())).Callback<Report>((x) => _reports.Add(x));
            reportRep.Setup(x => x.GetQuery(It.IsAny<string>())).Returns<string>((x) => _queries.FirstOrDefault(z => z.QueryGUID.Equals("Valid")));
            reportRep.Setup(x => x.Get(It.IsAny<string>())).Returns<string>((z) => _reports.SingleOrDefault(x => x.ReportGUID.Equals(z)));
            reportRep.Setup(x => x.Update(It.IsAny<Report>())).Callback<Report>((z) => { var origrep = _reports.SingleOrDefault(x => x.ReportGUID.Equals(z.ReportGUID)); origrep = z; });
            reportRep.Setup(x => x.Remove(It.IsAny<string>())).Returns<string>((z) => _reports.RemoveAll(x => x.ReportGUID == z) > 0);
            Mock<IReportUserRelRepository> reportUserRelRep = new Mock<IReportUserRelRepository>();
            reportUserRelRep.Setup(x => x.Add(It.IsAny<ReportUserRel>())).Callback<ReportUserRel>((x) => _reportUserRel.Add(x));
            reportUserRelRep.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<int>())).Returns<int, int>((repId, userId) => _reportUserRel.SingleOrDefault(x => x.User.Id == userId && x.Report.Id == repId));
            Mock<IReportDashboardRelRepository> repDashRel = new Mock<IReportDashboardRelRepository>();
            repDashRel.Setup(x => x.GetReportDashboards(It.IsAny<int>())).Returns<int>(z => _reportDashRel.Where(x => x.Report.Id == z).ToList());
            repDashRel.Setup(x => x.Remove(It.IsAny<int>())).Callback<int>(z => _reportDashRel.RemoveAll(x => x.Id == z));
            // Act
            ReportManager manager = new ReportManager(reportRep.Object, repDashRel.Object, null, null, null, reportUserRelRep.Object);

            // Assert
            Assert.Throws(typeof(PermissionException), () => manager.DeleteReport("Valid", _users[1]));
        }
    }
}