using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class UpdateDashboardDto
    {
        [Required(ErrorMessage = "Yout should provide DashboardGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string DashboardGUID { get; set; }
        [Required(ErrorMessage = "Yout should provide Name.")]
        [MaxLength(200, ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        public List<ReportDashboardRelDto> Reports { get; set; } = new List<ReportDashboardRelDto>();
    }
}
