using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos.DashboardDtos;

namespace szakdoga.Controllers
{
    //[Authorize]
    [Route("api/dashboards")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly IReportRepository _reportRepository;
        private readonly DashboardManager _manager;

        public DashboardController(IDashboardRepository dashboardRepository, IReportDashboardRelRepository repDashRel, IReportRepository reportRepository, DashboardManager manager)
        {
            _dashboardRepository = dashboardRepository;
            _reportDashboardRel = repDashRel;
            _reportRepository = reportRepository;
        }

        [HttpGet("GetStyle/{dashboardGUID}")]
        public IActionResult GetDashBoardStyle(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID!");

            var dashboardDto = _manager.GetDashBoardStyle(dashboardGUID);

            if (dashboardDto == null)
                return NotFound();
            else
                return Ok(dashboardDto);
        }

        [HttpPost("Create")]
        public IActionResult CreateDashboard([FromBody] CreateDashboardDto dbDto)
        {
            if (dbDto == null) return BadRequest("Invalid Dto");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var guid = _manager.CreateDashboard(dbDto);
            if (!string.IsNullOrEmpty(guid))
                return Created(string.Empty, guid);
            else
                return BadRequest("Could not save.");
        }

        [HttpPut("Update/{dashboardGUID}")]
        public IActionResult UpdateReport([FromBody] UpdateDashboardDto dashboard, string dashboardGUID)
        {
            if (dashboard == null) return BadRequest("Invalid Dto");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.UpdateDashboard(dashboard, dashboardGUID))
                return NoContent();
            else
                return BadRequest("Report GUID is not valid.");
        }

        [HttpDelete("Delete/{dashboardGUID}")]
        public IActionResult DeleteDashboard(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID!");

            if (_manager.DeleteDashboard(dashboardGUID))
                return NoContent();
            else
                return BadRequest();
        }

        [HttpGet("GetDashboardReportNames/{dashboardGUID}")]
        public IActionResult GetDashboardReportNames(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID");

            IEnumerable<ReportDto> repots = _manager.GetReportNames(dashboardGUID);
            return Ok(repots);
        }

        [HttpGet("GetDashboardReports/{dashboardGUID}")]
        public IActionResult GetDashboardReports(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID");

            var reports = _manager.GetDashboardReports(dashboardGUID);

            if (reports == null)
                return BadRequest();
            else
                return Ok(reports);
        }

        [HttpGet("GetDashboardReportPosition/{dashboardGUID}/{reportGUID}")]
        public IActionResult GetDashboardReportPosition(string dashboardGUID, string reportGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID) || string.IsNullOrEmpty(reportGUID))
                return BadRequest("Empty GUID");

            string position = _manager.GetPosition(dashboardGUID, reportGUID);

            if (string.IsNullOrEmpty(position))
                return BadRequest("Invalid dashboardGUID or reportGUID!");
            else
                return Ok(position);
        }
    }
}