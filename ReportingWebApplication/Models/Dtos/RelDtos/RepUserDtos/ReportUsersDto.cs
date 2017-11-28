using System.Collections.Generic;
using szakdoga.Models.Dtos.RelDtos.RepUserDtos;

namespace szakdoga.Models.Dtos.RelDtos
{
    public class ReportUsersDto
    {
        public string ReportGUID { get; set; }
        public IEnumerable<ReportUserDto> Users { get; set; }
    }
}