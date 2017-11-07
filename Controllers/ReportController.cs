using Microsoft.AspNetCore.Mvc;

namespace szakdoga.Controllers
{
    [Route("api/reports")]
    public class ReportController : Controller
    {
        [HttpGet()]
        public IActionResult GetCities()
        {
            return Ok("asd");
        }
    }
}
