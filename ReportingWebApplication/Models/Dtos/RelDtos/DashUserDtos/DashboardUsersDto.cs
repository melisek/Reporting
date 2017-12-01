using System.Collections.Generic;
using System.Linq;

namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class DashboardUsersDto
    {
        public string DashboardGUID { get; set; }
        public IEnumerable<DashboardUserDto> Users { get; set; }

        public override bool Equals(object obj)
        {
            var dashoardUsersDto = obj as DashboardUsersDto;
            if (dashoardUsersDto != null)
                return dashoardUsersDto.DashboardGUID == this.DashboardGUID && this.Users.SequenceEqual(dashoardUsersDto.Users);
            return false;
        }
    }
}