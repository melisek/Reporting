using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class User
    {
        //Stringlength
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public string EmailAddress { get; set; }
    }
}