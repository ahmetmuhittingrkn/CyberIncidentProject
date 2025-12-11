using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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

        public ICommand LoadAnalyticsCommand { get; }
        public ICommand RefreshCommand { get; }

        public AnalyticsViewModel()
        {
            _apiService = new ApiService();
            _incidentTypeStats = new ObservableCollection<IncidentTypeStats>();
            _severityStats = new ObservableCollection<SeverityStats>();
            _statusSummary = new ObservableCollection<StatusSummary>();

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
    }
}

