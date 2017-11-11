using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "Yout should provide UserGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }
        public string Name { get; set; }
    }
}
