using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos;

namespace szakdoga.Controllers
{
    [Route("api/dashboarduserrels")]
    public class DashboardUserRelController : Controller
    {
        DashboardUserRelManager _manager;
        ILogger _logger;
        public DashboardUserRelController(DashboardUserRelManager manager, ILogger<DashboardUserRelController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        [HttpGet("GetDashboardUsers/{dashboardGUID}")]
        public IActionResult GetDashboardUsers(string DashboardGUID)
        {
            if (string.IsNullOrEmpty(DashboardGUID)) return BadRequest("Empty GUID!");
            var DashboardUsers = _manager.GetDashboardUsers(DashboardGUID);

            if (DashboardUsers == null)
                BadRequest();

            return Ok(DashboardUsers);
        }
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateDashboardUserDto DashboardUserRel)
        {
            if (DashboardUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.Create(DashboardUserRel)) return Created(string.Empty, null);
            else return BadRequest("Could not save.");
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UpdateDashboardUserDto DashboardUserRel)
        {
            if (DashboardUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.UpdateDashboardUserRel(DashboardUserRel))
                return NoContent();
            else return BadRequest();

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] DeleteDashboardUserDto DashboardUserRel)
        {
            if (DashboardUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.DeleteDashboardUserRel(DashboardUserRel))
                return NoContent();
            else return BadRequest();
        }
    }
}