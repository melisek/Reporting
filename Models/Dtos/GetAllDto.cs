namespace szakdoga.Models.Dtos
{
    public class GetAllDto
    {
        public string Filter { get; set; }
        public int Page { get; set; }
        public int Rows { get; set; }
        public SortDto Sort { get; set; }
    }
}
