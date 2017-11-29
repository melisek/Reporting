using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class UpdateReportStyle
    {
        [Required(ErrorMessage = "ReportGUID is required.")]
        public string ReportGUID { get; set; }
        [Required(ErrorMessage = "Style is required.")]
        public string Style { get; set; }
    }
}
