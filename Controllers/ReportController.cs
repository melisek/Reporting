using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private readonly ReportManager _manager;
        private readonly ILogger<ReportController> _logger;

        public ReportController(ReportManager manager, ILogger<ReportController> logger)
        {
            _manager = manager;
            _logger = logger;
        }
        [HttpGet("Get/{reportGUID}")]
        public IActionResult Get(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) throw new BasicException("Empty GUID!");
                var report = _manager.GetReport(reportGUID);
                return Ok(report);
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

        [HttpGet("GetStyle/{reportGUID}")]
        public IActionResult GetRiportStyle(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) throw new BasicException("Empty GUID!");
                var report = _manager.GetReportStyle(reportGUID);

                return Ok(report);
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
        public IActionResult CreateReport([FromBody] CreateReportDto report)
        {
            try
            {
                if (report == null) throw new BasicException("Invalud input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var guid = _manager.CreateReport(report);
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

        [HttpPut("Update/{reportGUID}")]
        public IActionResult UpdateReport([FromBody] UpdateReportDto report, string reportGUID)
        {
            try
            {
                if (report == null) throw new BasicException("Invalid input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.UpdateReport(report, reportGUID);
                return NoContent();
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

        [HttpDelete("Delete/{reportGUID}")]
        public IActionResult DeleteReport(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) throw new BasicException("Empty GUID!");

                _manager.DeleteReport(reportGUID);
                return NoContent();
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

        [HttpPost("GetAll")]
        public IActionResult GetAll([FromBody] GetAllFilterDto filter)
        {
            try
            {
                if (filter == null) throw new BasicException("Wrong structure!");
                if (!ModelState.IsValid) BadRequest(ModelState);

                return Ok(_manager.GetAllReport(filter));
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

        [HttpPost("GetReportSource")]
        public IActionResult GetReportSource([FromBody] ReportSourceFilterDto filter)
        {
            try
            {
                if (filter == null) throw new BasicException("Wrong structure!");
                if (!ModelState.IsValid) BadRequest(ModelState);

                return Ok(_manager.GetQuerySource(filter));
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
    }
}