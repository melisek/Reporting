using System;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class Dashboard : BaseEntity, IComparable
    {
        [Required]
        [StringLength(50)]
        public string DashBoardGUID { get; set; }

        public string Style { get; set; }
        public User LastModifier { get; set; }
        public User Author { get; set; }

        public int CompareTo(object obj)
        {
            Dashboard dash = obj as Dashboard;
            if (dash != null)
            {
                return this.Name.CompareTo(dash.Name);
            }
            return 0;
        }
    }
}