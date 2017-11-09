using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class CreateReportDto
    {
        [Required]
        public string GUID { get; set; }
        [Required]
        public string Name { get; set; }
        public string QueryGUID { get; set; }
        public string[] Columns { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }

    }
}
