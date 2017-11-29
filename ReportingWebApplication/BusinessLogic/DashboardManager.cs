using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.ReportDtos;
using szakdoga.Others;

namespace szakdoga.BusinessLogic
{
    public class DashboardManager
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly IReportRepository _reportRepository;
        private readonly IUserDashboardRelRepository _userDashRelRepository;

        public DashboardManager(IDashboardRepository dashboardRepository, IReportDashboardRelRepository repDashRel, IReportRepository reportRepository, IUserDashboardRelRepository userDashRelRepository)
        {
            _dashboardRepository = dashboardRepository;
            _reportDashboardRel = repDashRel;
            _reportRepository = reportRepository;
            _userDashRelRepository = userDashRelRepository;
        }

        public DashboardDto GetDashBoardStyle(string dashboardGUID, User user)
        {
            var db = _dashboardRepository.Get(dashboardGUID);
            if (db == null)
                return null;
            else
            {
                var rel = _userDashRelRepository.Get(db.Id, user.Id);
                if (rel == null || !(rel.AuthoryLayer == (int)DashboardUserPermissions.CanModify || rel.AuthoryLayer == (int)DashboardUserPermissions.CanWatch))
                    throw new PermissionException("Don't have permission.");
                return Mapper.Map<DashboardDto>(db);
            }
        }

        public string CreateDashboard(CreateDashboardDto dbDto, User user)
        {
            Dashboard dashEnt = new Dashboard
            {
                DashBoardGUID = CreateGUID.GetGUID(),
                Name = dbDto.Name
            };
            _dashboardRepository.Add(dashEnt);//add után már benne van a Id érték
            _userDashRelRepository.Add(new UserDashboardRel { User = user, AuthoryLayer = (int)DashboardUserPermissions.CanModify, Dashboard = dashEnt });

            foreach (var rel in dbDto.Reports)
            {
                var report = _reportRepository.Get(rel.ReportGUID);
                if (report == null)
                    continue;
                _reportDashboardRel.Add(new ReportDashboardRel { Dashboard = dashEnt, Report = report, Position = rel.Position });
            }
            return dashEnt.DashBoardGUID;
        }

        public bool UpdateDashboard(UpdateDashboardDto dashboard, string dashboardGUID, User user)
        {
            var origDashboard = _dashboardRepository.Get(dashboardGUID);
            if (origDashboard == null)
                throw new NotFoundException("Invalid dashboardGUID");

            var perrel = _userDashRelRepository.Get(origDashboard.Id, user.Id);
            if (perrel == null || perrel.AuthoryLayer != (int)DashboardUserPermissions.CanModify)
                throw new PermissionException("Don't have permission.");


            origDashboard.Name = dashboard.Name;
            origDashboard.LastModifier = user;

            _dashboardRepository.Update(origDashboard);

            foreach (var rel in _reportDashboardRel.GetDashboardReports(origDashboard.Id))
            {
                _reportDashboardRel.Remove(rel.Id);
            }

            foreach (var rel in dashboard.Reports)
            {
                var report = _reportRepository.Get(rel.ReportGUID);
                if (report == null)
                    continue;
                _reportDashboardRel.Add(new ReportDashboardRel { Dashboard = origDashboard, Report = report, Position = rel.Position });
            }
            return true;
        }

        public bool DeleteDashboard(string dashboardGUID, User user)
        {
            var db = _dashboardRepository.Get(dashboardGUID);
            if (db == null)
                throw new NotFoundException("Invalid dashboardGUID");

            var perrel = _userDashRelRepository.Get(db.Id, user.Id);
            if (perrel == null || perrel.AuthoryLayer != (int)DashboardUserPermissions.CanModify)
                throw new PermissionException("Don't have permission.");

            foreach (var rel in _reportDashboardRel.GetDashboardReports(db.Id))
            {
                _reportDashboardRel.Remove(rel.Id);
            }
            return _dashboardRepository.Remove(dashboardGUID);
        }

        public IEnumerable<ReportDto> GetReportNames(string dashboardGUID, User user)
        {
            var dash = _dashboardRepository.Get(dashboardGUID);
            if (dash == null)
                throw new NotFoundException("Invalid dashboardGUID");

            var perrel = _userDashRelRepository.Get(dash.Id, user.Id);
            if (perrel == null || !(perrel.AuthoryLayer == (int)DashboardUserPermissions.CanModify || perrel.AuthoryLayer == (int)DashboardUserPermissions.CanWatch))
                throw new PermissionException("Don't have permission.");

            return Mapper.Map<IEnumerable<ReportDto>>(
                _reportDashboardRel.GetDashboardReports(dash.Id).
                Select(x => x.Report).
                Select(y => new Report { Name = y.Name, ReportGUID = y.ReportGUID }).ToList());
        }

        public string GetPosition(string dashboardGUID, string reportGUID)
        {
            var dash = _dashboardRepository.Get(dashboardGUID);
            if (dash == null) throw new NotFoundException("Invalid dashboardGUID");
            var rels = _reportDashboardRel.GetDashboardReports(dash.Id);
            if (rels == null) throw new NotFoundException("Invalid dashboardGUID");

            return rels.FirstOrDefault().Position;
        }

        public DashboardReportDto GetDashboardReports(string dashboardGUID, User user)
        {
            var dash = _dashboardRepository.Get(dashboardGUID);
            if (dash == null)
                throw new NotFoundException("Invalid dashboardGUID");

            var perrel = _userDashRelRepository.Get(dash.Id, user.Id);
            if (perrel == null || !(perrel.AuthoryLayer == (int)DashboardUserPermissions.CanModify || perrel.AuthoryLayer == (int)DashboardUserPermissions.CanWatch))
                throw new PermissionException("Don't have permission.");

            var rels = _reportDashboardRel.GetDashboardReports(dash.Id);

            var dashboardReportDto = new DashboardReportDto();
            dashboardReportDto.DashboardGUID = dashboardGUID;
            foreach (var rel in rels)
            {
                dashboardReportDto.Reports.Add(new ReportFullDto
                {
                    Name = rel.Report.Name,
                    ReportGUID = rel.Report.ReportGUID,
                    Postition = rel.Position,
                    Style = rel.Report.Style,
                    QueryGUID = rel.Report.Query.QueryGUID
                });
            }
            return dashboardReportDto;
        }

        public object GetAllDashboard(GetAllFilterDto filter, User user)
        {
            IEnumerable<int> rels = _userDashRelRepository.GetAll().Where(x => x.User == user).Select(y => y.Dashboard.Id).ToList();

            IEnumerable<Dashboard> dashboards = _dashboardRepository.GetAll()
                .Where(x => !x.Deleted &&
                            rels.Contains(x.Id)
                            && (String.IsNullOrEmpty(filter.Filter)
                            || x.Name.ToLower().Contains(filter.Filter.ToLower())
                            || (x.LastModifier != null && x.LastModifier.Name.ToLower().Contains(filter.Filter.ToLower()))
                            || (x.Author != null && x.Author.Name.ToLower().Contains(filter.Filter.ToLower())))).ToList();
            int count = dashboards.Count();

            if (filter.Sort.Direction == Direction.Asc)
                dashboards = dashboards
                     .OrderBy(z => typeof(Dashboard).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
                     .Skip(filter.Page > 1 ? (filter.Page - 1) * filter.Rows : 0)
                     .Take(filter.Rows).ToList();
            else
                dashboards = dashboards
                 .OrderByDescending(z => typeof(Dashboard).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
                 .Skip(filter.Page > 1 ? (filter.Page - 1) * filter.Rows : 0)
                 .Take(filter.Rows).ToList();



            return new AllDashboardDto
            {
                Dashboards = Mapper.Map<IEnumerable<DashboardForAllDto>>(dashboards).ToList(),
                TotalCount = count
            };
        }
    }
}