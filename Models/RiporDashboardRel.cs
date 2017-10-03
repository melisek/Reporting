using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class RiporDashboardRel
    {
        [Key]
        public int Id { get; set; }
        public Dashboard Dashboard { get; set; }
        public Riport Riport { get; set; }
        public string Position { get; set; }
    }
}