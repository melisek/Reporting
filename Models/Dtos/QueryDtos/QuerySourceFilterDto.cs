using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.QueryDtos
{
    public class QuerySourceFilterDto
    {
        [Required(ErrorMessage = "Yout should provide QueryGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

        [Required(ErrorMessage = "Rows are required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum value is 1.")]
        public int Rows { get; set; }

        [Required(ErrorMessage = "Page is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum value is 1.")]
        public int Page { get; set; }
        public SortDto Sort { get; set; }
        [Required(ErrorMessage = "Filter is required.")]
        public string Filter { get; set; }
        [Required(ErrorMessage = "Columns are required")]
        public string[] Columns { get; set; }
    }
}