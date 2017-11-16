using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.RelDtos.RepUserDtos;

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
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CreateReportUserDto reportUserRel)
        {
            if (reportUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.Create(reportUserRel)) return Created(string.Empty, null);
            else return BadRequest("Could not save.");
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UpdateReportUserDto reportUserRel)
        {
            if (reportUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.UpdateReportUserRel(reportUserRel))
                return NoContent();
            else return BadRequest();

        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromBody] DeleteReportUserDto reportUserRel)
        {
            if (reportUserRel == null) return BadRequest("Wrong body format.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.DeleteReportUserRel(reportUserRel))
                return NoContent();
            else return BadRequest();
        }

    }
}
