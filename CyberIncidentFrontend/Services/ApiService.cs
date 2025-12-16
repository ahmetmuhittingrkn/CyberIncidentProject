using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CyberIncidentWPF.Models;

namespace CyberIncidentWPF.Services
{
    /// <summary>
    /// REST API ile haberleşmeyi sağlayan servis katmanı.
    /// HttpClient best practices ve güvenlik standartlarına uygun şekilde yapılandırılmıştır.
    /// </summary>
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        // BaseAddress MUTLAKA "/" ile bitmeli - Relative URL birleştirme için kritik!
        private const string BaseUrl = "http://localhost:8080/api/";

        // Backend, LocalDateTime için offset/fraction içermeyen format bekliyor
        private const string BackendDateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss";
        
        // Merkezi JSON serileştirme ayarları - Tüm metotlarda tutarlılık için
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            Converters =
            {
                new BackendLocalDateTimeConverter(),
                new BackendNullableLocalDateTimeConverter()
            }
        };

        public ApiService()
        {
            var handler = new HttpClientHandler
            {
#if DEBUG
                // GÜVENLIK: Sertifika doğrulama SADECE geliştirme ortamında devre dışı
                // Production'da ASLA kullanılmamalı - MITM riskine karşı
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
#endif
                UseDefaultCredentials = false
            };
            
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Helper Methods

        /// <summary>
        /// Query parametrelerini URL-safe formata encode eder.
        /// Özel karakterler ve boşluklar içeren değerler için zorunlu.
        /// </summary>
        private string BuildQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || !parameters.Any())
                return string.Empty;

            var encodedParams = parameters
                .Where(p => !string.IsNullOrEmpty(p.Value))
                .Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}");

            return "?" + string.Join("&", encodedParams);
        }

        /// <summary>
        /// HTTP hata durumlarında response body'yi okuyarak anlamlı hata mesajı üretir.
        /// 4xx/5xx hatalarında detaylı bilgi sağlar.
        /// </summary>
        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response, string operationName)
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, JsonOptions) 
                    ?? throw new Exception($"{operationName}: Deserialization returned null");
            }

            // Hata durumunda response body'yi oku
            var errorContent = await response.Content.ReadAsStringAsync();
            var statusCode = (int)response.StatusCode;
            
            throw new HttpRequestException(
                $"{operationName} failed.\n" +
                $"Status Code: {statusCode} ({response.StatusCode})\n" +
                $"Reason: {response.ReasonPhrase}\n" +
                $"Details: {errorContent}");
        }

        private sealed class BackendLocalDateTimeConverter : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                // Backend "yyyy-MM-ddTHH:mm:ss" üretiyor; bunun dışındaki ISO formatları da tolere edelim
                if (reader.TokenType != JsonTokenType.String)
                    throw new JsonException("Expected string for DateTime.");

                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s))
                    return default;

                if (DateTime.TryParseExact(s, BackendDateTimeFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var exact))
                {
                    return DateTime.SpecifyKind(exact, DateTimeKind.Unspecified);
                }

                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var fallback))
                    return fallback;

                throw new JsonException($"Invalid DateTime value: '{s}'");
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                // Offset/fraction olmadan yaz
                writer.WriteStringValue(value.ToString(BackendDateTimeFormat, CultureInfo.InvariantCulture));
            }
        }

        private sealed class BackendNullableLocalDateTimeConverter : JsonConverter<DateTime?>
        {
            public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                    return null;
                if (reader.TokenType != JsonTokenType.String)
                    throw new JsonException("Expected string for nullable DateTime.");

                var s = reader.GetString();
                if (string.IsNullOrWhiteSpace(s))
                    return null;

                if (DateTime.TryParseExact(s, BackendDateTimeFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out var exact))
                {
                    return DateTime.SpecifyKind(exact, DateTimeKind.Unspecified);
                }

                if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var fallback))
                    return fallback;

                throw new JsonException($"Invalid DateTime value: '{s}'");
            }

            public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
            {
                if (value == null)
                {
                    writer.WriteNullValue();
                    return;
                }
                writer.WriteStringValue(value.Value.ToString(BackendDateTimeFormat, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// PATCH istekleri için manuel HttpRequestMessage oluşturur.
        /// .NET 5 öncesi sürümlerde PatchAsync desteği olmadığından zorunlu.
        /// </summary>
        private async Task<HttpResponseMessage> SendPatchAsync(string requestUri, HttpContent content)
        {
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri)
            {
                Content = content
            };
            
            return await _httpClient.SendAsync(request);
        }

        #endregion

        #region Incident Operations

        /// <summary>
        /// Tüm incidents'ları filtrelerle birlikte getirir.
        /// Query parametreleri otomatik olarak URL encode edilir.
        /// </summary>
        public async Task<List<Incident>> GetIncidentsAsync(string? type = null, 
            string? severity = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new Dictionary<string, string>();
                
                if (!string.IsNullOrEmpty(type))
                    queryParams["type"] = type;
                if (!string.IsNullOrEmpty(severity))
                    queryParams["severity"] = severity;
                if (startDate.HasValue)
                    queryParams["startDate"] = startDate.Value.ToString(BackendDateTimeFormat);
                if (endDate.HasValue)
                    queryParams["endDate"] = endDate.Value.ToString(BackendDateTimeFormat);

                var query = BuildQueryString(queryParams);
                var response = await _httpClient.GetAsync($"incidents{query}");
                
                return await HandleResponseAsync<List<Incident>>(response, "Get incidents") 
                    ?? new List<Incident>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get incidents: {ex.Message}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception("Request timeout: Backend did not respond in time", ex);
            }
        }

        /// <summary>
        /// ID'ye göre tek bir incident getirir.
        /// </summary>
        public async Task<Incident?> GetIncidentByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"incidents/{id}");
                return await HandleResponseAsync<Incident>(response, $"Get incident {id}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get incident {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Yeni incident oluşturur.
        /// </summary>
        public async Task<Incident> CreateIncidentAsync(Incident incident)
        {
            try
            {
                // Backend sadece bu alanları bekliyor
                var payload = new
                {
                    title = incident.Title,
                    description = incident.Description,
                    incidentType = incident.IncidentType,
                    severityLevel = incident.SeverityLevel,
                    incidentDate = incident.IncidentDate,
                    reporterId = incident.ReporterId,
                    iocs = incident.Iocs,
                    openedByAnalyst = incident.OpenedByAnalyst
                };

                var json = JsonSerializer.Serialize(payload, JsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("incidents", content);
                
                return await HandleResponseAsync<Incident>(response, "Create incident")
                    ?? throw new Exception("Create incident: Response was null");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to create incident: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Incident'ı günceller.
        /// </summary>
        public async Task<Incident> UpdateIncidentAsync(int id, Incident incident)
        {
            try
            {
                var json = JsonSerializer.Serialize(incident, JsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"incidents/{id}", content);
                
                return await HandleResponseAsync<Incident>(response, $"Update incident {id}")
                    ?? throw new Exception("Update incident: Response was null");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update incident {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Incident durumunu günceller (PATCH).
        /// HttpRequestMessage kullanılarak framework uyumluluğu sağlanır.
        /// </summary>
        public async Task UpdateIncidentStatusAsync(int incidentId, string status, string? analystName = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(new { status, analystName }, JsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // PATCH desteği olmayan .NET sürümleri için manuel request
                var response = await SendPatchAsync($"incidents/{incidentId}/status", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(
                        $"Update status failed: {(int)response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to update incident status: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Incident'ı siler.
        /// </summary>
        public async Task DeleteIncidentAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"incidents/{id}");
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException(
                        $"Delete failed: {(int)response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to delete incident {id}: {ex.Message}", ex);
            }
        }

        #endregion

        #region User Operations

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("users");
                return await HandleResponseAsync<List<User>>(response, "Get users")
                    ?? new List<User>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get users: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// ID'ye göre kullanıcı getirir.
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"users/{id}");
                return await HandleResponseAsync<User>(response, $"Get user {id}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get user {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur.
        /// </summary>
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user, JsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("users", content);
                
                return await HandleResponseAsync<User>(response, "Create user")
                    ?? throw new Exception("Create user: Response was null");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to create user: {ex.Message}", ex);
            }
        }

        #endregion

        #region Analytics Operations

        /// <summary>
        /// Incident tip istatistiklerini getirir.
        /// </summary>
        public async Task<List<IncidentTypeStats>> GetIncidentTypeStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("analytics/incident-types");
                var dict = await HandleResponseAsync<Dictionary<string, int>>(response, "Get incident type stats");
                return dict?
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => new IncidentTypeStats { IncidentType = kvp.Key, Count = kvp.Value })
                    .ToList()
                    ?? new List<IncidentTypeStats>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get incident type stats: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Severity istatistiklerini getirir.
        /// </summary>
        public async Task<List<SeverityStats>> GetSeverityStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("analytics/severity-stats");
                var dict = await HandleResponseAsync<Dictionary<string, int>>(response, "Get severity stats");
                return dict?
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => new SeverityStats { SeverityLevel = kvp.Key, Count = kvp.Value })
                    .ToList()
                    ?? new List<SeverityStats>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get severity stats: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kritik incident sayısını getirir.
        /// </summary>
        public async Task<int> GetCriticalIncidentCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("analytics/critical-count");
                var result = await HandleResponseAsync<Dictionary<string, int>>(response, "Get critical count");
                return result?.GetValueOrDefault("criticalCount", 0) ?? 0;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get critical incident count: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Durum özetini getirir.
        /// </summary>
        public async Task<List<StatusSummary>> GetStatusSummaryAsync()
        {
            try
            {
                // UI tarafı StatusSummary listesi bekliyor; backend bu veriyi /status-stats ile Map olarak dönüyor
                var response = await _httpClient.GetAsync("analytics/status-stats");
                var dict = await HandleResponseAsync<Dictionary<string, int>>(response, "Get status stats");
                return dict?
                    .OrderByDescending(kvp => kvp.Value)
                    .Select(kvp => new StatusSummary { Status = kvp.Key, Count = kvp.Value })
                    .ToList()
                    ?? new List<StatusSummary>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get status summary: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Zaman çizelgesi verilerini getirir.
        /// </summary>
        public async Task<List<TimelineData>> GetTimelineDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("analytics/timeline");
                return await HandleResponseAsync<List<TimelineData>>(response, "Get timeline data")
                    ?? new List<TimelineData>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get timeline data: {ex.Message}", ex);
            }
        }

        #endregion

        #region IDisposable Pattern (Optional - HttpClient Lifecycle Management)

        /// <summary>
        /// HttpClient kaynaklarını temizler.
        /// NOT: Singleton kullanımında Dispose çağrılmamalı!
        /// </summary>
        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        #endregion
    }
}
