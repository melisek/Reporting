using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class ReportDiagramDiscDto
    {
        [Required(ErrorMessage = "ReportGUID is required.")]
        public string ReportGUID { get; set; }

        [Required(ErrorMessage = "NameColumn is required.")]
        public string NameColumn { get; set; }

        [Required(ErrorMessage = "ValueColumn is required.")]
        public string ValueColumn { get; set; }

        [Required(ErrorMessage = "Aggreagation is required.")]
        public Aggregation Aggregation { get; set; }
    }
}