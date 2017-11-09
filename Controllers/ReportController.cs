using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models;

namespace szakdoga.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("GetStyle/{reportGUID}")]
        public IActionResult GetStyle(string reportGUID)
        {
            using (var reportManager = new ReportManager(_reportRepository))
            {
                var report = reportManager.GetReportStyle(reportGUID);
                if (report == null)
                    return NotFound();
                else
                    return Ok(reportManager.GetReportStyle(reportGUID));//OK 200 as státuszkódja van
            }
        }
    }
}
