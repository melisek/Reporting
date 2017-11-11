using System.ComponentModel.DataAnnotations;

namespace szakdoga
{
    public class ReportDto
    {
        [Required(ErrorMessage = "Yout should provide GUID.")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string ReportGUID { get; set; }

        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        public string Style { get; set; }
    }
}