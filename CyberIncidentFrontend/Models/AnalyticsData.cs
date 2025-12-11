using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CyberIncidentWPF.Models
{
    public class IncidentTypeStats
    {
        [JsonPropertyName("incidentType")]
        public string IncidentType { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class SeverityStats
    {
        [JsonPropertyName("severityLevel")]
        public string SeverityLevel { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class StatusSummary
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }

    public class TimelineData
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}

