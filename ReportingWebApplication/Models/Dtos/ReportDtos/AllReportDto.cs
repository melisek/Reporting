using System.Collections.Generic;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class AllReportDto
    {
        public int TotalCount { get; set; }
        public List<ReportForAllDto> Reports { get; set; } = new List<ReportForAllDto>();
    }
}