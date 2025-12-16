using System;
using System.Text.Json.Serialization;

namespace CyberIncidentWPF.Models
{
    public class Incident
    {
        [JsonPropertyName("incidentId")]
        public int IncidentId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("incidentType")]
        public string IncidentType { get; set; } = string.Empty;

        [JsonPropertyName("severityLevel")]
        public string SeverityLevel { get; set; } = string.Empty;

        [JsonPropertyName("incidentDate")]
        public DateTime IncidentDate { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "OPEN";

        [JsonPropertyName("reporterId")]
        public int ReporterId { get; set; }

        [JsonPropertyName("reporterName")]
        public string? ReporterName { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("resolvedAt")]
        public DateTime? ResolvedAt { get; set; }

        [JsonPropertyName("iocs")]
        public string? Iocs { get; set; }

        [JsonPropertyName("openedByAnalyst")]
        public string? OpenedByAnalyst { get; set; }

        [JsonPropertyName("closedByAnalyst")]
        public string? ClosedByAnalyst { get; set; }
    }
}

