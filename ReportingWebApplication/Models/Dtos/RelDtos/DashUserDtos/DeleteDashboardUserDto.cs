using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class DeleteDashboardUserDto
    {
        [Required(ErrorMessage = "DashboardGUID is requiered.")]
        public string DashboardGUID { get; set; }

        [Required(ErrorMessage = "UserGUID is requiered.")]
        public string UserGUID { get; set; }
    }
}