using System;
using System.Collections.Generic;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class AllReportDto
    {
        public int TotalCount { get => Reports.Count; }
        public List<ReportDto> Reports { get; set; } = new List<ReportDto>();
        public QueryDto Query { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public UserDto Author { get; set; }
    }
}