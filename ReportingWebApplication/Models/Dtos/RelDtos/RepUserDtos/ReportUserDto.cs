namespace szakdoga.Models.Dtos.RelDtos.RepUserDtos
{
    public class ReportUserDto
    {
        public string UserGUID { get; set; }
        public string Name { get; set; }
        public ReportUserPermissions Permission { get; set; }
    }
}