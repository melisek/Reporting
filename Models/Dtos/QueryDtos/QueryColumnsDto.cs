using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.QueryDtos
{
    public class QueryColumnsDto
    {
        [Required(ErrorMessage = "Required QueryGUID.")]
        public string QueryGUID { get; set; }

        public ColumnNamesDto[] Columns { get; set; }
    }
}