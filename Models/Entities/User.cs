using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class User : BaseEntity
    {
        //Stringlength max 255, enélkül a string-eket nvarcharként képezi le az adatbázisban-> erre nem lehet indexeket rakni
        [Required]
        [StringLength(50)]
        public string UserGUID { get; set; }

        [Required]
        public string Password { get; set; }

        public string EmailAddress { get; set; }
    }
}