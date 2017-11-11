using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class Report : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string ReportGUID { get; set; }

        public Query Query { get; set; }

        /// <summary>
        /// A grafikonok adataihoz
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// :-vel tagolt string "tömb"
        /// </summary>
        public string Columns { get; set; }

        public string Filter { get; set; }
        public string Sort { get; set; }
        public int Rows { get; set; }
    }
}