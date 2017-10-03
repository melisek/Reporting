namespace szakdoga.Models
{
    public class RiporDashboardRel
    {
        public int Id { get; set; }
        public Dashboard Dashboard { get; set; }
        public Riport Riport { get; set; }
        public string Position { get; set; }
    }
}