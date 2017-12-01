namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class DashboardUserDto
    {
        public string UserGUID { get; set; }
        public string Name { get; set; }
        public DashboardUserPermissions Permission { get; set; }

        public override bool Equals(object obj)
        {
            var dashboardUserDto = obj as DashboardUserDto;
            if (dashboardUserDto != null)
                return this.UserGUID == dashboardUserDto.UserGUID;
            return false;
        }
    }
}