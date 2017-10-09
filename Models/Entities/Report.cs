namespace szakdoga.Models
{
    public class Report : BaseEntity
    {
        public Query Query { get; set; }

        public string Style { get; set; }
    }
}