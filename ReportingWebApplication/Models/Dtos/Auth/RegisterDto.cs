using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EmailAddress is required")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}