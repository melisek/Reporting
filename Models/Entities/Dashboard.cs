using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class Dashboard : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string DashBoardGUID { get; set; }
        public string Style { get; set; }
    }
}