using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CyberIncidentWPF.Controls;
using CyberIncidentWPF.Helpers;
using CyberIncidentWPF.Models;
using CyberIncidentWPF.Services;

namespace CyberIncidentWPF.ViewModels
{
    public class AnalyticsViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private ObservableCollection<IncidentTypeStats> _incidentTypeStats;
        private ObservableCollection<SeverityStats> _severityStats;
        private ObservableCollection<StatusSummary> _statusSummary;
        private int _totalIncidents;
        private int _openIncidents;
        private int _criticalIncidents;
        private int _resolvedIncidents;
        private bool _isLoading;

        public ObservableCollection<IncidentTypeStats> IncidentTypeStats
        {
            get => _incidentTypeStats;
            set => SetProperty(ref _incidentTypeStats, value);
        }

        public ObservableCollection<SeverityStats> SeverityStats
        {
            get => _severityStats;
            set => SetProperty(ref _severityStats, value);
        }

        public ObservableCollection<StatusSummary> StatusSummary
        {
            get => _statusSummary;
            set => SetProperty(ref _statusSummary, value);
        }

        public int TotalIncidents
        {
            get => _totalIncidents;
            set => SetProperty(ref _totalIncidents, value);
        }

        public int OpenIncidents
        {
            get => _openIncidents;
            set => SetProperty(ref _openIncidents, value);
        }

        public int CriticalIncidents
        {
            get => _criticalIncidents;
            set => SetProperty(ref _criticalIncidents, value);
        }

        public int ResolvedIncidents
        {
            get => _resolvedIncidents;
            set => SetProperty(ref _resolvedIncidents, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public List<PieChartItem> IncidentTypeChartData { get; private set; }
        public List<PieChartItem> SeverityChartData { get; private set; }
        public List<BarChartItem> StatusChartData { get; private set; }

        public ICommand LoadAnalyticsCommand { get; }
        public ICommand RefreshCommand { get; }

        public AnalyticsViewModel()
        {
            // Singleton pattern - HttpClient socket tükenmesini önler
            _apiService = ApiServiceProvider.Instance;
            _incidentTypeStats = new ObservableCollection<IncidentTypeStats>();
            _severityStats = new ObservableCollection<SeverityStats>();
            _statusSummary = new ObservableCollection<StatusSummary>();
            IncidentTypeChartData = new List<PieChartItem>();
            SeverityChartData = new List<PieChartItem>();
            StatusChartData = new List<BarChartItem>();

            LoadAnalyticsCommand = new RelayCommand(async _ => await LoadAnalyticsAsync());
            RefreshCommand = new RelayCommand(async _ => await LoadAnalyticsAsync());

            // Load analytics on initialization - with error handling
            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () => 
            {
                await LoadAnalyticsAsync();
            });
        }

        private async Task LoadAnalyticsAsync()
        {
            try
            {
                IsLoading = true;

                // Load all analytics data in parallel
                var typeStatsTask = _apiService.GetIncidentTypeStatsAsync();
                var severityStatsTask = _apiService.GetSeverityStatsAsync();
                var statusSummaryTask = _apiService.GetStatusSummaryAsync();
                var criticalCountTask = _apiService.GetCriticalIncidentCountAsync();
                var allIncidentsTask = _apiService.GetIncidentsAsync();

                await Task.WhenAll(typeStatsTask, severityStatsTask, statusSummaryTask, 
                    criticalCountTask, allIncidentsTask);

                // Update UI with results
                IncidentTypeStats = new ObservableCollection<IncidentTypeStats>(await typeStatsTask);
                SeverityStats = new ObservableCollection<SeverityStats>(await severityStatsTask);
                StatusSummary = new ObservableCollection<StatusSummary>(await statusSummaryTask);
                CriticalIncidents = await criticalCountTask;

                var allIncidents = await allIncidentsTask;
                TotalIncidents = allIncidents.Count;

                // Calculate counts from status summary
                OpenIncidents = 0;
                ResolvedIncidents = 0;
                foreach (var status in StatusSummary)
                {
                    if (status.Status == "OPEN")
                        OpenIncidents = status.Count;
                    else if (status.Status == "RESOLVED")
                        ResolvedIncidents = status.Count;
                }

                // Prepare chart data
                UpdateChartData();
            }
            catch (Exception ex)
            {
                // Show error but don't crash
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"Analytics verisi alınamadı.\n\n" +
                        $"Hata: {ex.Message}\n\n" +
                        $"Backend çalışıyor mu kontrol edin.", 
                        "Backend Bağlantı Hatası", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning);
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateChartData()
        {
            // Incident Type Chart Data
            var typeColors = new Dictionary<string, Color>
            {
                { "PHISHING", Color.FromRgb(99, 102, 241) },      // Indigo
                { "MALWARE", Color.FromRgb(239, 68, 68) },         // Red
                { "DATA_BREACH", Color.FromRgb(245, 158, 11) },    // Amber
                { "DDOS", Color.FromRgb(236, 72, 153) },           // Pink
                { "UNAUTHORIZED_ACCESS", Color.FromRgb(139, 92, 246) }, // Purple
                { "INSIDER_THREAT", Color.FromRgb(20, 184, 166) }, // Teal
                { "OTHER", Color.FromRgb(107, 114, 128) }          // Gray
            };

            IncidentTypeChartData = IncidentTypeStats.Select(stat => new PieChartItem
            {
                Label = stat.IncidentType,
                Value = stat.Count,
                Color = typeColors.ContainsKey(stat.IncidentType) 
                    ? typeColors[stat.IncidentType] 
                    : Color.FromRgb(156, 163, 175)
            }).ToList();

            OnPropertyChanged(nameof(IncidentTypeChartData));

            // Severity Chart Data
            var severityColors = new Dictionary<string, Color>
            {
                { "CRITICAL", Color.FromRgb(220, 38, 38) },   // Red
                { "HIGH", Color.FromRgb(234, 88, 12) },      // Orange
                { "MEDIUM", Color.FromRgb(202, 138, 4) },    // Yellow
                { "LOW", Color.FromRgb(5, 150, 105) }        // Green
            };

            SeverityChartData = SeverityStats.Select(stat => new PieChartItem
            {
                Label = stat.SeverityLevel,
                Value = stat.Count,
                Color = severityColors.ContainsKey(stat.SeverityLevel)
                    ? severityColors[stat.SeverityLevel]
                    : Color.FromRgb(156, 163, 175)
            }).ToList();

            OnPropertyChanged(nameof(SeverityChartData));

            // Status Chart Data
            var statusColors = new Dictionary<string, Color>
            {
                { "OPEN", Color.FromRgb(59, 130, 246) },           // Blue
                { "IN_PROGRESS", Color.FromRgb(245, 158, 11) },    // Amber
                { "RESOLVED", Color.FromRgb(16, 185, 129) },       // Green
                { "CLOSED", Color.FromRgb(107, 114, 128) }         // Gray
            };

            StatusChartData = StatusSummary.Select(stat => new BarChartItem
            {
                Label = stat.Status.Replace("_", " "),
                Value = stat.Count,
                Color = statusColors.ContainsKey(stat.Status)
                    ? statusColors[stat.Status]
                    : Color.FromRgb(156, 163, 175)
            }).OrderByDescending(s => s.Value).ToList();

            OnPropertyChanged(nameof(StatusChartData));
        }
    }
}

