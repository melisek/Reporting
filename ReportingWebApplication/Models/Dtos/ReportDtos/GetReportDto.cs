using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class GetReportDto
    {
        [Required(ErrorMessage = "You should provide Name.")]
        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You should provide QueryGUID")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

        public string[] Columns { get; set; }
        public string Filter { get; set; }
        public SortDto Sort { get; set; }
        public int Rows { get; set; }
    }
}