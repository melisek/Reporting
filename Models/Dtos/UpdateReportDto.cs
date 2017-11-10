using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class UpdateReportDto
    {
        [Required(ErrorMessage = "Yout should provide Name.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Yout should provide QueryGUID")]
        public string QueryGUID { get; set; }
        public string[] Columns { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string ReportGUID { get; set; }
        public int Rows { get; set; }
    }
}
