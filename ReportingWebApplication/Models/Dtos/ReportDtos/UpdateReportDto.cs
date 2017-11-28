using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class UpdateReportDto
    {
        [Required(ErrorMessage = "Yout should provide Name.")]
        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yout should provide QueryGUID")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

        public string[] Columns { get; set; }
        public string Filter { get; set; }
        public SortDto Sort { get; set; }

        [Required(ErrorMessage = "Yout should provide ReportGUID")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string ReportGUID { get; set; }

        public int Rows { get; set; }
    }
}