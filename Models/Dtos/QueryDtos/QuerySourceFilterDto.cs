using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos.QueryDtos
{
    public class QuerySourceFilterDto
    {
        [Required(ErrorMessage = "Yout should provide QueryGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

        [Required(ErrorMessage = "X is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum values is 1.")]
        public int X { get; set; }

        [Required(ErrorMessage = "Y is required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Minimum values is 1.")]
        public int Y { get; set; }
    }
}