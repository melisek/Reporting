using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace szakdoga.Models.Dtos
{
    public class SortDto
    {
        public string ColumnName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Direction Direction { get; set; }
    }
}