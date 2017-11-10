using System.ComponentModel.DataAnnotations;
using szakdoga.Models;

namespace szakdoga
{
    public class ReportDto
    {
        [Required(ErrorMessage = "Yout should provide GUID.")]
        public string GUID { get; set; }
        public Query Query { get; set; }

        public string Style { get; set; }
    }
}
