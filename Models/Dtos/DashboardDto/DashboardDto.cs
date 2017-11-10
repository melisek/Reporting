using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.DashboardDto
{
    public class DashboardDto
    {
        [Required]
        public string GUID { get; set; }
        public string Style { get; set; }
    }
}
