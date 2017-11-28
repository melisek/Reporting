using System;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.DashboardDtos
{
    public class DashboardForAllDto
    {
        [Required(ErrorMessage = "Yout should provide GUID.")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string DashboardGUID { get; set; }

        [MaxLength(ErrorMessage = "Maximum length is 200 characters.")]
        public string Name { get; set; }

        public QueryDto Query { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public UserDto LastModifier { get; set; }
        public UserDto Author { get; set; }
    }
}
