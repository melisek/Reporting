using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.Controllers
{
    [Route("api/queries")]
    public class QueryController : Controller
    {
        private readonly QueryManager _manager;

        public QueryController(QueryManager manager)
        {
            _manager = manager;
        }

        [HttpGet("GetAll")]
        public IActionResult GetQueries()
        {
            var queries = _manager.GetAll();
            return Ok(queries);
        }

        [HttpGet("GetQueryColumns/{queryGUID}")]
        public IActionResult GetQueryColumns(string queryGUID)
        {
            if (string.IsNullOrEmpty(queryGUID))
                return BadRequest();

            var result = _manager.GetColumnNames(queryGUID);
            return Ok(result);
        }

        [HttpPost("GetQuerySource")]
        public IActionResult GetQuerySource([FromBody] QuerySourceFilterDto filter)
        {
            if (filter == null)
                BadRequest();
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            var jsonResult = _manager.GetQuerySource(filter);

            return Ok(jsonResult);
        }

        [HttpGet("GetQueryColumnCount/{queryGUID}")]
        public IActionResult GetQueryColumnCount(string queryGUID)
        {
            if (string.IsNullOrEmpty(queryGUID))
                return BadRequest("Empty queryGUID");

            return Ok(_manager.GetQueryColumnCount(queryGUID));
        }
    }
}