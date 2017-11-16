namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class DashboardUserDto
    {
        public string UserGUID { get; set; }
        public string Name { get; set; }
        public DashboardUserPermissions Permission { get; set; }
    }
}