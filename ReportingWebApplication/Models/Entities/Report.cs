using System;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models
{
    public class Report : BaseEntity, IComparable
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
        public User LastModifier { get; set; }
        public User Author { get; set; }

        public int CompareTo(object obj)
        {
            Report report = obj as Report;
            if (report!=null)
            {
                return this.Name.CompareTo(report.Name);
            }
            return 0;
        }
    }
}