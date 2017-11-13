using System;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class ReportForAllDto
    {
        public QueryDto Query { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public UserDto LastModifier { get; set; }
        public UserDto Author { get; set; }
    }
}
