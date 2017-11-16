using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.RelDtos.RepUserDtos
{
    public class DeleteReportUserDto
    {
        [Required(ErrorMessage = "RepostGUID is required.")]
        public string ReportGUID { get; set; }

        [Required(ErrorMessage = "UserGUID is required.")]
        public string UserGUID { get; set; }
    }
}