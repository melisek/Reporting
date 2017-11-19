﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.DashboardDtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.BusinessLogic
{
    public class DashboardManager
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly IReportRepository _reportRepository;

        public DashboardManager(IDashboardRepository dashboardRepository, IReportDashboardRelRepository repDashRel, IReportRepository reportRepository)
        {
            _dashboardRepository = dashboardRepository;
            _reportDashboardRel = repDashRel;
            _reportRepository = reportRepository;
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
                throw new NotFoundException("Invalid dashboardGUID");
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
            foreach (var rel in _reportDashboardRel.GetDashboardReports(_dashboardRepository.Get(dashboardGUID).Id))
            {
                _reportDashboardRel.Remove(rel.Id);
            }
            return _dashboardRepository.Remove(dashboardGUID);
        }

        public IEnumerable<ReportDto> GetReportNames(string dashboardGUID)
        {
            var dash = _dashboardRepository.Get(dashboardGUID);
            if (dash == null)
                throw new NotFoundException("Invalid dashboardGUID");

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

        public DashboardReportDto GetDashboardReports(string dashboardGUID)
        {
            var dash = _dashboardRepository.Get(dashboardGUID);
            if (dash == null)
                throw new NotFoundException("Invalid dashboardGUID");

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

        public object GetAllDashboard(GetAllFilterDto filter)
        {

            IEnumerable<Dashboard> dashboards = dashboards = _dashboardRepository.GetAll()
                             .Where(x => (String.IsNullOrEmpty(filter.Filter) || x.Name.ToLower().Contains(filter.Filter.ToLower()))).ToList();

            int count = dashboards.Count();

            if (filter.Sort.Direction == Direction.Asc)
                dashboards = dashboards
                     .OrderBy(z => typeof(Report).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
                     .Skip(filter.Page > 1 ? (filter.Page - 1) * filter.Rows : 0)
                     .Take(filter.Rows).ToList();
            else
                dashboards = dashboards
                 .OrderByDescending(z => typeof(Report).GetProperty(filter.Sort.ColumnName).GetValue(z, null))
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