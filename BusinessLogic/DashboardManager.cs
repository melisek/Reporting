using AutoMapper;
using System;
using szakdoga.Models;
using szakdoga.Models.Dtos.DashboardDtos;

namespace szakdoga.BusinessLogic
{
    public class DashboardManager : IDisposable
    {
        private IDashboardRepository _dashboardRepository;
        private IReportDashboardRelRepository _reportDashboardRel;
        private IReportRepository _reportRepository;

        public DashboardManager(IDashboardRepository dashboardRepository, IReportDashboardRelRepository repDashRel, IReportRepository reportRepository)
        {
            _dashboardRepository = dashboardRepository;
            _reportDashboardRel = repDashRel;
            _reportRepository = reportRepository;
        }

        public void Dispose()
        {
            _dashboardRepository = null;
        }

        public DashboardDto GetDashBoardStyle(string dashboardGUID)
        {
            var db = _dashboardRepository.Get(dashboardGUID);
            if (db == null)
                return null;
            else
                return Mapper.Map<DashboardDto>(db);
        }

        public string CreateDashboard(CreateDashboardDto dbDto)
        {
            Dashboard dashEnt = new Dashboard
            {
                DashBoardGUID = CreateGUID.GetGUID(),
                Name = dbDto.Name
            };
            _dashboardRepository.Add(dashEnt);//add után már benne van a Id érték

            foreach (var rel in dbDto.Reports)
            {
                var report = _reportRepository.Get(rel.ReportGUID);
                if (report == null)
                    continue;
                _reportDashboardRel.Add(new ReportDashboardRel { Dashboard = dashEnt, Report = report, Position = rel.Position });
            }
            return dashEnt.DashBoardGUID;
        }

        public bool UpdateDashboard(UpdateDashboardDto dashboard, string dashboardGUID)
        {
            var origDashboard = _dashboardRepository.Get(dashboardGUID);
            if (origDashboard == null)
                return false;
            var dashboardEntity = new Dashboard
            {
                Name = dashboard.Name,
                DashBoardGUID = dashboard.DashboardGUID,
            };

            _dashboardRepository.Update(dashboardEntity);

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

        public bool DeleteDashboard(string dashboardGUID)
        {
            return _dashboardRepository.Remove(dashboardGUID);
        }
    }
}