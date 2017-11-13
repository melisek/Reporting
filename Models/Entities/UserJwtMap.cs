using System;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Entities
{
    public class UserJwtMap
    {
        [Key]
        public int Id { get; set; }
        public string Jwt { get; set; }
        public User User { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
