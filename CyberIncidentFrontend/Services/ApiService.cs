using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CyberIncidentWPF.Models;

namespace CyberIncidentWPF.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:8080/api";

        public ApiService()
        {
            // Configure HttpClientHandler for better compatibility
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                UseDefaultCredentials = false
            };
            
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Incident Operations

        // Get all incidents with optional filters
        public async Task<List<Incident>> GetIncidentsAsync(string? type = null, 
            string? severity = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(type)) queryParams.Add($"type={type}");
                if (!string.IsNullOrEmpty(severity)) queryParams.Add($"severity={severity}");
                if (startDate.HasValue) queryParams.Add($"startDate={startDate:yyyy-MM-dd}");
                if (endDate.HasValue) queryParams.Add($"endDate={endDate:yyyy-MM-dd}");

                var query = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
                var response = await _httpClient.GetAsync($"/incidents{query}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Incident>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Incident>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get incidents: {ex.Message}", ex);
            }
        }

        // Get incident by ID
        public async Task<Incident?> GetIncidentByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/incidents/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Incident>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get incident: {ex.Message}", ex);
            }
        }

        // Create new incident
        public async Task<Incident> CreateIncidentAsync(Incident incident)
        {
            try
            {
                var json = JsonSerializer.Serialize(incident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/incidents", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Incident>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new Exception("Failed to deserialize created incident");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create incident: {ex.Message}", ex);
            }
        }

        // Update incident
        public async Task<Incident> UpdateIncidentAsync(int id, Incident incident)
        {
            try
            {
                var json = JsonSerializer.Serialize(incident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/incidents/{id}", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Incident>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new Exception("Failed to deserialize updated incident");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update incident: {ex.Message}", ex);
            }
        }

        // Update incident status
        public async Task UpdateIncidentStatusAsync(int incidentId, string status)
        {
            try
            {
                var json = JsonSerializer.Serialize(new { status });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync($"/incidents/{incidentId}/status", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update incident status: {ex.Message}", ex);
            }
        }

        // Delete incident
        public async Task DeleteIncidentAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/incidents/{id}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete incident: {ex.Message}", ex);
            }
        }

        #endregion

        #region User Operations

        // Get all users
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/users");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<User>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get users: {ex.Message}", ex);
            }
        }

        // Get user by ID
        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/users/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user: {ex.Message}", ex);
            }
        }

        // Create user
        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/users", content);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<User>(responseJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new Exception("Failed to deserialize created user");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create user: {ex.Message}", ex);
            }
        }

        #endregion

        #region Analytics Operations

        // Get incident count by type
        public async Task<List<IncidentTypeStats>> GetIncidentTypeStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/analytics/incident-types");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<IncidentTypeStats>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<IncidentTypeStats>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get incident type stats: {ex.Message}", ex);
            }
        }

        // Get severity statistics
        public async Task<List<SeverityStats>> GetSeverityStatsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/analytics/severity-stats");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<SeverityStats>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<SeverityStats>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get severity stats: {ex.Message}", ex);
            }
        }

        // Get critical incident count
        public async Task<int> GetCriticalIncidentCountAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/analytics/critical-count");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Dictionary<string, int>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return result?.GetValueOrDefault("criticalCount", 0) ?? 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get critical incident count: {ex.Message}", ex);
            }
        }

        // Get status summary
        public async Task<List<StatusSummary>> GetStatusSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/analytics/status-summary");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<StatusSummary>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<StatusSummary>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get status summary: {ex.Message}", ex);
            }
        }

        // Get timeline data
        public async Task<List<TimelineData>> GetTimelineDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/analytics/timeline");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TimelineData>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<TimelineData>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get timeline data: {ex.Message}", ex);
            }
        }

        #endregion
    }
}

