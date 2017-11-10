using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;

namespace szakdoga.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("GetStyle/{reportGUID}")]
        public IActionResult GetStyle(string reportGUID)
        {
            using (var reportManager = new ReportManager(_reportRepository))
            {
                var report = reportManager.GetReportStyle(reportGUID);
                if (report == null)
                    return NotFound();
                else
                    return Ok(reportManager.GetReportStyle(reportGUID));//OK 200 as státuszkódja van
            }
        }

        [HttpPost("CreateReport")]
        public IActionResult CreateReport([FromBody] CreateReportDto report)
        {
            if (report == null      //ha nem lehet a bemeneti json-t serializálni a megadott objektumba akk null lesz az értéke
                || !ModelState.IsValid)//ha nem felelt meg a DataAnnotation atribútumoknak - nem a legjobb adatellenőrzésre a dataannotations, mert keverve vannak az ellenőrzési helye
                //hozzá lehet adni itt is hibát, ellenőrzést, ajánlás: FluenValidation:  library- lambdákkal lehet megkötéseket definiálni
                return BadRequest(); //400-as hibakód

            using (var reportManager = new ReportManager(_reportRepository))
            {
                var guid = reportManager.CreateReport(report);
                if (!string.IsNullOrEmpty(guid))
                    return Created(string.Empty, guid);
                else
                    return BadRequest("Could not save.");
                //Created()//lehetne még createatute()-akkor megadná h hogy tudja elérni tehát: GetReport/ReportGUID
            }

        }

        [HttpPut("UpdateReport/{reportGUID}")]
        public IActionResult UpdateReport([FromBody] UpdateReportDto report, string reportGUID)
        {
            if (report == null || !ModelState.IsValid)
                return BadRequest();

            using (var reportManager = new ReportManager(_reportRepository))
            {
                if (reportManager.UpdateReport(report, reportGUID))
                    return NoContent();
                else
                    return BadRequest("Report GUID is not valid.");
            }
        }

        [HttpDelete("DeleteReport/{reportGUID}")]
        public IActionResult DeleteReport(string reportGUID)
        {
            if (string.IsNullOrEmpty(reportGUID))
                return BadRequest();

            using (var reportManager = new ReportManager(_reportRepository))
            {
                if (reportManager.DeleteReport(reportGUID))
                    return NoContent();
                else
                    return BadRequest();

            }

        }
    }
}
