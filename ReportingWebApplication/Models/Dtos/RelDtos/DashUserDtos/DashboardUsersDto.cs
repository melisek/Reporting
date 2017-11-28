using System.Collections.Generic;

namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class DashboardUsersDto
    {
        public string DashboardGUID { get; set; }
        public IEnumerable<DashboardUserDto> Users { get; set; }
    }
}