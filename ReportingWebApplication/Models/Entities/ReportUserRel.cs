using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class ReportUserRel
    {
        [Key]
        public int Id { get; set; }

        public Report Report { get; set; }
        public User User { get; set; }

        public int AuthoryLayer { get; set; }
    }
}