using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.RelDtos.RepUserDtos
{
    public class CreateReportUserDto
    {
        [Required(ErrorMessage = "ReportGUID is requiered.")]
        public string ReportGUID { get; set; }

        [Required(ErrorMessage = "UserGUID is requiered.")]
        public string UserGUID { get; set; }

        [Required(ErrorMessage = "Permission is requiered.")]
        [Range(0, 2, ErrorMessage = "Permission value must be between 0 and 2.")]
        public int Permission { get; set; }
    }
}