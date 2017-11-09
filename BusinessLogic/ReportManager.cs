using System;
using szakdoga.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

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
            var report = _reportRepository.GetAll().FirstOrDefault(x=>x.GUID==reportGUID);

            if (report == null)
                return null;

            return new ReportDto { GUID = report.GUID, Style = report.Style };
        }
    }
}
