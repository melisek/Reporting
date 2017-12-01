using System.Collections.Generic;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class AllDashboardDto
    {
        public int TotalCount { get; set; }
        public List<DashboardForAllDto> Dashboards { get; set; } = new List<DashboardForAllDto>();
    }
}