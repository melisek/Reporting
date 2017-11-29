using System.Collections.Generic;
using szakdoga.Models.Dtos.ReportDtos;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class DashboardReportDto
    {
        public string DashboardGUID { get; set; }
        public List<ReportFullDto> Reports { get; set; } = new List<ReportFullDto>();
    }
}