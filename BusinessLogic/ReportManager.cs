using AutoMapper;
using System;
using System.Linq;
using szakdoga.Models;
using szakdoga.Models.Dtos;

namespace szakdoga.BusinessLogic
{
    public class ReportManager : IDisposable
    {
        private IReportRepository _reportRepository;
        public ReportManager(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
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
            var origReport = _reportRepository.Get(reportGUID);
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
            return _reportRepository.Remove(reportGUID);
        }
    }
}
