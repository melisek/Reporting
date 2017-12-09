using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;
using szakdoga.Others;

namespace szakdoga.Controllers
{
    [Authorize]
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private readonly ReportManager _manager;
        private readonly ILogger<ReportController> _logger;
        private readonly IUserRepository _userRep;

        public ReportController(ReportManager manager, ILogger<ReportController> logger, IUserRepository userrep)
        {
            _manager = manager;
            _logger = logger;
            _userRep = userrep;
        }

        [HttpGet("Get/{reportGUID}")]
        public IActionResult Get(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) throw new BasicException("Empty GUID!");
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
                var report = _manager.GetReport(reportGUID, user);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
                var report = _manager.GetReportStyle(reportGUID, user);

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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                if (report == null) throw new BasicException("Invalid input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);

                var guid = _manager.CreateReport(report, user);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);

                _manager.UpdateReport(report, reportGUID, user);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
                _manager.DeleteReport(reportGUID, user);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
                return Ok(_manager.GetAllReport(filter, user));
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
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
                User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
                return Ok(_manager.GetQuerySource(filter, user));
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("UpdateStyle")]
        public IActionResult UpdateStyle([FromBody] UpdateReportStyle report)
        {
            try
            {
                if (report == null) throw new BasicException("Invalid input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.UpdateStyle(report);
                return Ok();
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("GetDiscreetRiportDiagram")]
        public IActionResult GetDiscreteReportDiagram([FromBody]ReportDiagramDiscDto diagram)
        {
            try
            {
                if (diagram == null) throw new BasicException("Invalid input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = _manager.GetDiscreetRiportDiagram(diagram);
                return Ok(result);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("GetSeriesRiportDiagram")]
        public IActionResult GetSeriesReportDiagram([FromBody]ReportDiagramSerDto diagram)
        {
            try
            {
                if (diagram == null) throw new BasicException("Invalud input format!");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var result = _manager.GetSeriesRiportDiagram(diagram);
                return Ok(result);
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
            catch (PermissionException ex)
            {
                _logger.LogError(ex.Message);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [HttpGet("Export/{reportGUID}")]
        public FileResult Export(string reportGUID)
        {
            string fileName = String.Empty;
            User user = _userRep.GetByEmailAdd(this.User.Claims.SingleOrDefault(x => x.Type == "EmailAddress").Value);
            byte[] fileBytes = _manager.GetReportExportFile(reportGUID, user, out fileName);
            fileName = $"{fileName}.csv";
            return File(fileBytes, "text/csv", fileName);
        }
    }
}