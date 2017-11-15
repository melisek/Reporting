using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.BusinessLogic
{
    public class ReportManager : IDisposable
    {
        private IReportRepository _reportRepository;
        private IReportDashboardRelRepository _reportDashboardRel;
        private QueryManager _queryman;

        public ReportManager(IReportRepository reportRepository, IReportDashboardRelRepository repDashRel, QueryManager queryman)
        {
            _reportRepository = reportRepository;
            _reportDashboardRel = repDashRel;
            _queryman = queryman;
        }

        public void Dispose()
        {
            _reportRepository = null;
        }

        public ReportDto GetReportStyle(string reportGUID)
        {
            var report = _reportRepository.GetAll().FirstOrDefault(x => x.ReportGUID == reportGUID);

            if (report == null)
                return null;

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
                return false;
            var reportEntity = new Report
            {
                Name = report.Name,
                ReportGUID = report.ReportGUID,
                Query = _reportRepository.GetQuery(report.QueryGUID),
                Columns = StringArraySerializer(report.Columns),
                Filter = report.Filter,
                Sort = report.Sort,
                Rows = report.Rows
            };
            _reportRepository.Update(reportEntity);
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
            return _queryman.GetQuerySource(new Models.Dtos.QueryDtos.QuerySourceFilterDto { QueryGUID = riport.Query.QueryGUID, X = filter.X, Y = filter.Y });
        }
    }
}