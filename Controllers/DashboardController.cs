using Microsoft.AspNetCore.Authorization;
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
        private IDashboardRepository _dashboardRepository;
        private IReportDashboardRelRepository _reportDashboardRel;
        private IReportRepository _reportRepository;

        public DashboardController(IDashboardRepository dashboardRepository, IReportDashboardRelRepository repDashRel, IReportRepository reportRepository)
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
            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                var dashboardDto = dashboardManager.GetDashBoardStyle(dashboardGUID);

                if (dashboardDto == null)
                    return NotFound();
                else
                    return Ok(dashboardDto);
            }
        }

        [HttpPost("Create")]
        public IActionResult CreateDashboard([FromBody] CreateDashboardDto dbDto)
        {
            if (dbDto == null) return BadRequest("Invalid Dto");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                var guid = dashboardManager.CreateDashboard(dbDto);
                if (!string.IsNullOrEmpty(guid))
                    return Created(string.Empty, guid);
                else
                    return BadRequest("Could not save.");
            }
        }

        [HttpPut("Update/{dashboardGUID}")]
        public IActionResult UpdateReport([FromBody] UpdateDashboardDto dashboard, string dashboardGUID)
        {
            if (dashboard == null) return BadRequest("Invalid Dto");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                if (dashboardManager.UpdateDashboard(dashboard, dashboardGUID))
                    return NoContent();
                else
                    return BadRequest("Report GUID is not valid.");
            }
        }

        [HttpDelete("Delete/{dashboardGUID}")]
        public IActionResult DeleteDashboard(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID!");

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                if (dashboardManager.DeleteDashboard(dashboardGUID))
                    return NoContent();
                else
                    return BadRequest();
            }
        }

        [HttpGet("GetDashboardReportNames/{dashboardGUID}")]
        public IActionResult GetDashboardReportNames(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID");

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                IEnumerable<ReportDto> repots = dashboardManager.GetReportNames(dashboardGUID);
                return Ok(repots);
            }

        }

        [HttpGet("GetDashboardReports/{dashboardGUID}")]
        public IActionResult GetDashboardReports(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest("Empty GUID");

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                var reports = dashboardManager.GetDashboardReports(dashboardGUID);

                if (reports == null)
                    return BadRequest();
                else
                    return Ok(reports);
            }
        }

        [HttpGet("GetDashboardReportPosition/{dashboardGUID}/{reportGUID}")]
        public IActionResult GetDashboardReportPosition(string dashboardGUID, string reportGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID) || string.IsNullOrEmpty(reportGUID))
                return BadRequest("Empty GUID");

            using (var dashboardManager = new DashboardManager(_dashboardRepository, _reportDashboardRel, _reportRepository))
            {
                string position = dashboardManager.GetPosition(dashboardGUID, reportGUID);

                if (string.IsNullOrEmpty(position))
                    return BadRequest("Invalid dashboardGUID or reportGUID!");

                else
                    return Ok(position);
            }
        }
    }
}