using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class GetAllFilterDto
    {
        //[Required(ErrorMessage = "Filter is required.")]
        public string Filter { get; set; }

        [Required(ErrorMessage = "Page is required.")]
        public int Page { get; set; }

        [Required(ErrorMessage = "Row number is required.")]
        public int Rows { get; set; }

        public SortDto Sort { get; set; }
    }
}