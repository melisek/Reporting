using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.DashboardDtos;

namespace szakdoga.Controllers
{
    //[Authorize]
    [Route("api/dashboards")]
    public class DashboardController : Controller
    {
        private readonly DashboardManager _manager;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(DashboardManager manager, ILogger<DashboardController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpGet("GetStyle/{dashboardGUID}")]
        public IActionResult GetDashBoardStyle(string dashboardGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(dashboardGUID)) throw new BasicException("Empty GUID!");

                var dashboardDto = _manager.GetDashBoardStyle(dashboardGUID);
                return Ok(dashboardDto);
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

        [HttpPost("Create")]
        public IActionResult CreateDashboard([FromBody] CreateDashboardDto dbDto)
        {
            try
            {
                if (dbDto == null) throw new BasicException("Invalid Dto");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var guid = _manager.CreateDashboard(dbDto);
                return Created(string.Empty, guid);
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }


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