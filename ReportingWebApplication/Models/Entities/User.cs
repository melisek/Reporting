using System;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class User : BaseEntity, IComparable
    {
        //Stringlength max 255, enélkül a string-eket nvarcharként képezi le az adatbázisban-> erre nem lehet indexeket rakni
        [Required]
        [StringLength(50)]
        public string UserGUID { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        public int CompareTo(object obj)
        {
            User user = obj as User;
            if (user != null)
            {
                return this.Name.CompareTo(user.Name);
            }
            return 0;
        }
    }
}