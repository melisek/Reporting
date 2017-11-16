using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IReportDashboardRelRepository _reportDashboardRel;
        private readonly ReportManager _manager;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IReportRepository reportRepository, IReportDashboardRelRepository repDashRel, ReportManager manager, ILogger<ReportController> logger)
        {
            _reportRepository = reportRepository;
            _reportDashboardRel = repDashRel;
            _manager = manager;
            _logger = logger;
        }

        [HttpGet("GetStyle/{reportGUID}")]
        public IActionResult GetRiportStyle(string reportGUID)
        {
            try
            {
                if (string.IsNullOrEmpty(reportGUID)) return BadRequest("Empty GUID!");
                var report = _manager.GetReportStyle(reportGUID);
                if (report == null)
                    return NotFound();
                else
                    return Ok(_manager.GetReportStyle(reportGUID));//OK 200 as státuszkódja van
            }
            catch (BasicException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
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
            if (report == null)     //ha nem lehet a bemeneti json-t serializálni a megadott objektumba akk null lesz az értéke
                return BadRequest("Invalid Dto!");
            if (!ModelState.IsValid)//ha nem felelt meg a DataAnnotation atribútumoknak - nem a legjobb adatellenőrzésre a dataannotations, mert keverve vannak az ellenőrzési helye
                                    //hozzá lehet adni itt is hibát, ellenőrzést, ajánlás: FluenValidation:  library- lambdákkal lehet megkötéseket definiálni
                return BadRequest(ModelState); //400-as hibakód

            var guid = _manager.CreateReport(report);
            if (!string.IsNullOrEmpty(guid))
                return Created(string.Empty, guid);
            else
                return BadRequest("Could not save.");
            //Created()//lehetne még createatute()-akkor megadná h hogy tudja elérni tehát: GetReport/ReportGUID            
        }

        [HttpPut("Update/{reportGUID}")]
        public IActionResult UpdateReport([FromBody] UpdateReportDto report, string reportGUID)
        {
            if (report == null) return BadRequest("Invalid Dto");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (_manager.UpdateReport(report, reportGUID))
                return NoContent();
            else
                return BadRequest("Report GUID is not valid.");
        }

        [HttpDelete("Delete/{reportGUID}")]
        public IActionResult DeleteReport(string reportGUID)
        {
            if (string.IsNullOrEmpty(reportGUID))
                return BadRequest("Empty GUID!");

            if (_manager.DeleteReport(reportGUID))
                return NoContent();
            else
                return BadRequest();
        }

        [HttpPost("GetAll")]
        public IActionResult GetAll([FromBody] GetAllDto filter)
        {
            if (filter == null) return BadRequest("Wrong structure!");
            if (!ModelState.IsValid) BadRequest(ModelState);

            AllReportDto report = _manager.GetAllReport();
            if (report == null)
                return NotFound();
            else
                return Ok(report);
        }

        [HttpPost("GetReportSource")]
        public IActionResult GetReportSource([FromBody] ReportSourceFilterDto filter)
        {
            if (filter == null) return BadRequest("Wrong structure!");
            if (!ModelState.IsValid) BadRequest(ModelState);

            var jsonResult = _manager.GetQuerySource(filter);

            return Ok(jsonResult);
        }
    }
}