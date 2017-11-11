using System.ComponentModel.DataAnnotations;

namespace szakdoga
{
    public class QueryDto
    {
        [Required(ErrorMessage = "Yout should provide QueryGUID!")]
        [MaxLength(50, ErrorMessage = "Maximum GUID length is 50 characters.")]
        public string QueryGUID { get; set; }

        public string Name { get; set; }
    }
}
