using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos;
using szakdoga.Others;

namespace szakdoga.Controllers
{
    [Authorize]
    [Route("api/dashboarduserrels")]
    public class DashboardUserRelController : Controller
    {
        private DashboardUserRelManager _manager;
        private ILogger _logger;

        public DashboardUserRelController(DashboardUserRelManager manager, ILogger<DashboardUserRelController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpGet("GetDashboardUsers/{dashboardGUID}")]
        public IActionResult GetDashboardUsers(string DashboardGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(DashboardGUID)) throw new BasicException("Empty GUID!");
                var DashboardUsers = _manager.GetDashboardUsers(DashboardGUID);

                return Ok(DashboardUsers);
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
        public IActionResult Create([FromBody] CreateDashboardUserDto DashboardUserRel)
        {
            try
            {
                if (DashboardUserRel == null) throw new BasicException("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.Create(DashboardUserRel);
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

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UpdateDashboardUserDto DashboardUserRel)
        {
            try
            {
                if (DashboardUserRel == null) throw new BasicException("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.UpdateDashboardUserRel(DashboardUserRel);
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

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] DeleteDashboardUserDto DashboardUserRel)
        {
            try
            {
                if (DashboardUserRel == null) throw new Exception("Wrong body format.");
                if (!ModelState.IsValid) return BadRequest(ModelState);

                _manager.DeleteDashboardUserRel(DashboardUserRel);
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
    }
}