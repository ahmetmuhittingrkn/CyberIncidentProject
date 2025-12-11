using System.Text.Json.Serialization;

namespace CyberIncidentWPF.Models
{
    public class IncidentType
    {
        [JsonPropertyName("typeId")]
        public int TypeId { get; set; }

        [JsonPropertyName("typeName")]
        public string TypeName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}

