using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.BusinessLogic
{
    public class ReportManager
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly QueryManager _queryManager;

        public ReportManager(IReportRepository reportRepository, IReportDashboardRelRepository repDashRel, QueryManager queryman)
        {
            _reportRepository = reportRepository;
            _reportDashboardRel = repDashRel;
            _queryManager = queryman;
        }

        public ReportDto GetReportStyle(string reportGUID)
        {
            var report = _reportRepository.GetAll().FirstOrDefault(x => x.ReportGUID == reportGUID);

            if (report == null)
                throw new NotFoundException("Invalid reportGUID.");

            return Mapper.Map<ReportDto>(report);
        }

        public string CreateReport(CreateReportDto report)
        {
            var dbReport = new Report
            {
                Name = report.Name,
                ReportGUID = CreateGUID.GetGUID(),
                Query = _reportRepository.GetQuery(report.QueryGUID),
                Columns = StringArraySerializer(report.Columns),
                Filter = report.Filter,
                Sort = report.Sort,
                Rows = report.Rows
            };
            _reportRepository.Add(dbReport);

            return dbReport.ReportGUID;
        }

        private string StringArraySerializer(string[] array)
        {
            string result = String.Empty;

            if (array.Length == 0)
                return result;
            result += array[0];

            for (int i = 1; i < array.Length; i++)
            {
                result += ":" + array[i];
            }

            return result;
        }

        private string[] StringArrayDeserializer(string columns)
        {
            string[] result = columns.Split(':');
            return result;
        }

        public bool UpdateReport(UpdateReportDto report, string reportGUID)
        {
            var origReport = _reportRepository.Get(reportGUID);//TODO:origreportot kéne az update-nek adni, akk megoldott lenne a style fv módosítás
            if (origReport == null)
                throw new NotFoundException("Invalid reportGUID.");

            origReport.Name = report.Name;
            origReport.ReportGUID = report.ReportGUID;
            origReport.Query = _reportRepository.GetQuery(report.QueryGUID);
            origReport.Columns = StringArraySerializer(report.Columns);
            origReport.Filter = report.Filter;
            origReport.Sort = report.Sort;
            origReport.Rows = report.Rows;

            _reportRepository.Update(origReport);
            return true;
        }

        public bool DeleteReport(string reportGUID)
        {
            foreach (var rel in _reportDashboardRel.GetReportDashboards(_reportRepository.Get(reportGUID).Id))
            {
                _reportDashboardRel.Remove(rel.Id);
            }
            return _reportRepository.Remove(reportGUID);
        }

        public AllReportDto GetAllReport()
        {
            return new AllReportDto
            {
                Reports = Mapper.Map<IEnumerable<ReportForAllDto>>(_reportRepository.GetAll()).ToList()
            };
        }

        public object GetQuerySource(ReportSourceFilterDto filter)
        {
            var riport = _reportRepository.Get(filter.ReportGUID);
            return _queryManager.GetQuerySource(new Models.Dtos.QueryDtos.QuerySourceFilterDto { QueryGUID = riport.Query.QueryGUID, X = filter.X, Y = filter.Y });
        }
    }
}