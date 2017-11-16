using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;

namespace szakdoga.Controllers
{
    [Route("api/reportuserrels")]
    public class ReportUserRelController : Controller
    {
        private readonly ReportUserRelManager _manager;
        public ReportUserRelController(ReportUserRelManager reportUserRelManager)
        {
            _manager = reportUserRelManager;
        }

        [HttpGet("GetReportUsers/{reportGUID}")]
        public IActionResult GetReportUsers(string reportGUID)
        {
            if (string.IsNullOrEmpty(reportGUID)) return BadRequest("Empty GUID!");
            var reportUsers = _manager.GetReportUsers(reportGUID);

            if (reportUsers == null)
                BadRequest();

            return Ok(reportUsers);
        }

    }
}
