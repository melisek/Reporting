using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.QueryDtos
{
    public class ReportDashboardRelDto
    {
        [Required(ErrorMessage = "You should provide reportGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string ReportGUID { get; set; }

        public string Position { get; set; }
    }
}