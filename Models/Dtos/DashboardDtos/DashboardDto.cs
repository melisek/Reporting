using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class DashboardDto
    {
        [Required(ErrorMessage = "Yout should provide DashboardGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string DashboardGUID { get; set; }

        public string Style { get; set; }
    }
}