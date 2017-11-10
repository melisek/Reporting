using System.ComponentModel.DataAnnotations;

namespace szakdoga
{
    public class ReportDto
    {
        [Required(ErrorMessage = "Yout should provide GUID.")]
        public string GUID { get; set; }
        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }
        public string Style { get; set; }
    }
}
