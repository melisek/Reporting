using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class User
    {
        //Stringlength max 255, enélkül a string-eket nvarcharként képezi le az adatbázisban-> erre nem lehet indexeket rakni
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        public string EmailAddress { get; set; }
    }
}