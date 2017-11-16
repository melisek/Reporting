using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.ReportDtos
{
    public class ReportFullDto
    {
        [Required(ErrorMessage = "Yout should provide GUID.")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string ReportGUID { get; set; }

        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        public string Style { get; set; }
        public string Postition { get; set; }
        public string QueryGUID { get; set; }
    }
}