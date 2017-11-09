namespace szakdoga.Models
{
    public class Report : BaseEntity
    {
        public Query Query { get; set; }

        public string Style { get; set; }

        /// <summary>
        /// :-vel tagolt string "tömb"
        /// </summary>
        public string Columns { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }
}