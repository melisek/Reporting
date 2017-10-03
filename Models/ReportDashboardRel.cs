using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class ReportDashboardRel
    {
        [Key]
        public int Id { get; set; }
        public Dashboard Dashboard { get; set; }
        public Report Report { get; set; }
        public string Position { get; set; }
    }
}