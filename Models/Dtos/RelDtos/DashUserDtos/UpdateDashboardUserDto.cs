using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.RelDtos.DashboardUserRelDtos
{
    public class UpdateDashboardUserDto
    {
        [Required(ErrorMessage = "DashboardGUID is requiered.")]
        public string DashboardGUID { get; set; }

        [Required(ErrorMessage = "UserGUID is requiered.")]
        public string UserGUID { get; set; }

        [Required(ErrorMessage = "Permission is requiered.")]
        [Range(0, 2, ErrorMessage = "Permission value must be between 0 and 2.")]
        public int Permission { get; set; }
    }
}