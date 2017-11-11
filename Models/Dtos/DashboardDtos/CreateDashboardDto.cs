using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using szakdoga.Models.Dtos.QueryDtos;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class CreateDashboardDto
    {
        [Required(ErrorMessage = "Yout should provide Name.")]
        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        public List<ReportDashboardRelDto> Reports { get; set; } = new List<ReportDashboardRelDto>();
    }
}
