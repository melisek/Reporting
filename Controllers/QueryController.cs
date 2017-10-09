using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;

namespace szakdoga.Controllers
{
   // [Route("api/queries")]
    public class QueryController : Controller
    {
        private IQueryRepository _queryRepository;

        public QueryController(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }
        [HttpGet("api/query")]
        public JsonResult GetQueries()
        {
            //var queryIdsAndNames = _queryRepository.GetAll().Select(x => new { x.Name, x.Id });
            return new JsonResult(new List<object>()
            {
                new { id=1, Name="asd"}
            });
        }

    }
}
