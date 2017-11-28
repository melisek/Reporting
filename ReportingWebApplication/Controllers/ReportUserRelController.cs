using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.RelDtos.RepUserDtos;

namespace szakdoga.Controllers
{
    [Authorize]
    [Route("api/reportuserrels")]
    public class ReportUserRelController : Controller
    {
        private readonly ReportUserRelManager _manager;
        private readonly ILogger<ReportUserRelController> _logger;

        public ReportUserRelController(ReportUserRelManager reportUserRelManager, ILogger<ReportUserRelController> logger)
        {
            _manager = reportUserRelManager;
            _logger = logger;
        }

        [HttpGet("GetReportUsers/{reportGUID}")]
        public IActionResult GetReportUsers(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) throw new BasicException("Empty GUID!");

                return Ok(_manager.GetReportUsers(reportGUID));
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
        public IActionResult Create([FromBody] CreateReportUserDto reportUserRel)
        {
            try
            {
                if (reportUserRel == null) throw new BasicException("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.Create(reportUserRel);
                return Created(string.Empty, null);
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

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UpdateReportUserDto reportUserRel)
        {
            try
            {
                if (reportUserRel == null) throw new BasicException("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.UpdateReportUserRel(reportUserRel);
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

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] DeleteReportUserDto reportUserRel)
        {
            try
            {
                if (reportUserRel == null) throw new BasicException("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.DeleteReportUserRel(reportUserRel);
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
    }
}