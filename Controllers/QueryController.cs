using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using szakdoga.Models;

namespace szakdoga.Controllers
{
    [Route("api/queries")]
    public class QueryController : Controller
    {
        private IQueryRepository _queryRepository;

        public QueryController(IQueryRepository queryRepository)
        {
            _queryRepository = queryRepository;
        }
        [HttpGet()]
        public IActionResult GetQueries()
        {
            List<QueryDto> asd = _queryRepository.GetAll().Select(x => new QueryDto { Name= x.Name, QueryGUID= x.GUID }).ToList();
            return Ok(asd);
            /*
             * using blokkokban haszn�ljam a businesslogikot
             * dot-k a controller �s a manager k�z�tt
             * businesslogisban van a valid�tic�
             */
        }

    }
}
