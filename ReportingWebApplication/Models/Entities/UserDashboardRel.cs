using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class UserDashboardRel
    {
        [Key]
        public int Id { get; set; }

        public Dashboard Dashboard { get; set; }
        public User User { get; set; }
        public int AuthoryLayer { get; set; }
    }
}