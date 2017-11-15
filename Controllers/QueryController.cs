using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.Controllers
{
    [Route("api/queries")]
    public class QueryController : Controller
    {
        private IQueryRepository _queryRepository;
        private readonly QueryManager _manager;

        public QueryController(IQueryRepository queryRepository, QueryManager manager)
        {
            _queryRepository = queryRepository;
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
    }
}