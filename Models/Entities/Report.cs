using System.ComponentModel.DataAnnotations.Schema;

namespace szakdoga.Models
{
    public class Report : BaseEntity
    {
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
        [NotMapped]
        public string QueryGUID { get => Query.GUID; }
    }
}