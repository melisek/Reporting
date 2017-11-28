using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace szakdoga.Models.Dtos
{
    public class SortDto
    {
        [Required(ErrorMessage = "Columname is required.")]
        public string ColumnName { get; set; }

        [Required(ErrorMessage = "Direction is required.")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; set; }
    }
}