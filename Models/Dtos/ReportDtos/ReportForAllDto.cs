using System;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class ReportForAllDto
    {
        public QueryDto Query { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModifier { get; set; }
        public UserDto Author { get; set; }
    }
}
