using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class ReportDiagramSerDto
    {
        [Required(ErrorMessage = "ReportGUID is required.")]
        public string ReportGUID { get; set; }
        [Required(ErrorMessage = "NameColumn is required.")]
        public string NameColumn { get; set; }
        [Required(ErrorMessage = "SeriesNameColumn is required.")]
        public string SeriesNameColumn { get; set; }
        [Required(ErrorMessage = "ValueColumn is required.")]
        public string ValueColumn { get; set; }
    }
}
