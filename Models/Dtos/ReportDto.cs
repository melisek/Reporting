using System.ComponentModel.DataAnnotations;
using szakdoga.Models;

namespace szakdoga
{
    public class ReportDto
    {
        [Required]
        public string GUID { get; set; }
        public Query Query { get; set; }

        public string Style { get; set; }
    }
}
