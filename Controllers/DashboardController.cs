using Microsoft.AspNetCore.Mvc;
using szakdoga.BusinessLogic;
using szakdoga.Models;

namespace szakdoga.Controllers
{
    [Route("api/dashboards")]
    public class DashboardController : Controller
    {
        private IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;

        }

        [HttpGet("GetDashboardStyle/{dashboardGUID}")]
        public IActionResult GetDashBoardStyle(string dashboardGUID)
        {
            if (string.IsNullOrEmpty(dashboardGUID))
                return BadRequest();
            using (var dashboardManager = new DashboardManager(_dashboardRepository))
            {
                var dashboardDto = dashboardManager.GetDashBoardStyle(dashboardGUID);

                if (dashboardDto == null)
                    return NotFound();
                else
                    return Ok(dashboardDto);
            }
        }
    }
}
