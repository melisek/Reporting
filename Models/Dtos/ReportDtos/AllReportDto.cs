using System.Collections.Generic;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class AllReportDto
    {
        public int TotalCount { get => Reports.Count; }
        public List<ReportForAllDto> Reports { get; set; } = new List<ReportForAllDto>();
    }
}