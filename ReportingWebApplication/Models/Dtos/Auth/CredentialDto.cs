using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class CredentialDto
    {


        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}