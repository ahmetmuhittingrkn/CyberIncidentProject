using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CyberIncidentWPF.Helpers;
using CyberIncidentWPF.Models;
using CyberIncidentWPF.Services;

namespace CyberIncidentWPF.ViewModels
{
    public class IncidentListViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private ObservableCollection<Incident> _incidents;
        private Incident? _selectedIncident;
        private bool _isLoading;
        private string _searchText = string.Empty;
        private string? _selectedType;
        private string? _selectedSeverity;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public ObservableCollection<Incident> Incidents
        {
            get => _incidents;
            set => SetProperty(ref _incidents, value);
        }

        public Incident? SelectedIncident
        {
            get => _selectedIncident;
            set => SetProperty(ref _selectedIncident, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        public string? SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        public string? SelectedSeverity
        {
            get => _selectedSeverity;
            set => SetProperty(ref _selectedSeverity, value);
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public ObservableCollection<string> IncidentTypes { get; }
        public ObservableCollection<string> SeverityLevels { get; }
        public ObservableCollection<string> StatusList { get; }

        public ICommand LoadIncidentsCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ICommand ClearFiltersCommand { get; }
        public ICommand DeleteIncidentCommand { get; }
        public ICommand EditIncidentCommand { get; }
        public ICommand UpdateStatusCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand SendEmailCommand { get; }

        public IncidentListViewModel()
        {
            // Singleton pattern - HttpClient socket tükenmesini önler
            _apiService = ApiServiceProvider.Instance;
            _incidents = new ObservableCollection<Incident>();

            IncidentTypes = new ObservableCollection<string>
            {
                "PHISHING", "UNAUTHORIZED_ACCESS", "MALWARE", "DATA_BREACH",
                "DOS_ATTACK", "SOCIAL_ENGINEERING", "RANSOMWARE", "INSIDER_THREAT", "OTHER"
            };

            SeverityLevels = new ObservableCollection<string>
            {
                "LOW", "MEDIUM", "HIGH", "CRITICAL"
            };

            StatusList = new ObservableCollection<string>
            {
                "OPEN", "IN_PROGRESS", "RESOLVED", "CLOSED"
            };

            LoadIncidentsCommand = new RelayCommand(async _ => await LoadIncidentsAsync());
            ApplyFiltersCommand = new RelayCommand(async _ => await ApplyFiltersAsync());
            ClearFiltersCommand = new RelayCommand(_ => ClearFilters());
            DeleteIncidentCommand = new RelayCommand<int>(async id => await DeleteIncidentAsync(id));
            EditIncidentCommand = new RelayCommand<Incident>(EditIncident);
            UpdateStatusCommand = new RelayCommand<string>(async status => await UpdateStatusAsync(status));
            ViewDetailsCommand = new RelayCommand<Incident>(ViewDetails);
            SendEmailCommand = new RelayCommand<Incident>(SendEmail);

            // Load incidents on initialization - with error handling
            System.Windows.Application.Current.Dispatcher.InvokeAsync(async () => 
            {
                await LoadIncidentsAsync();
            });
        }

        private async Task LoadIncidentsAsync()
        {
            try
            {
                IsLoading = true;
                var incidents = await _apiService.GetIncidentsAsync();
                Incidents = new ObservableCollection<Incident>(incidents);
            }
            catch (Exception ex)
            {
                // Show error but don't crash the app
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show(
                        $"Backend'den veri alınamadı.\n\n" +
                        $"Hata: {ex.Message}\n\n" +
                        $"Lütfen backend'in çalıştığından emin olun:\n" +
                        $"http://localhost:8080/api/incidents", 
                        "Backend Bağlantı Hatası", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Warning);
                });
                
                // Initialize with empty list instead of crashing
                Incidents = new ObservableCollection<Incident>();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ApplyFiltersAsync()
        {
            try
            {
                IsLoading = true;
                var incidents = await _apiService.GetIncidentsAsync(
                    SelectedType, SelectedSeverity, StartDate, EndDate);

                // Apply search filter locally
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    incidents = incidents.Where(i =>
                        i.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        i.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                Incidents = new ObservableCollection<Incident>(incidents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ClearFilters()
        {
            SelectedType = null;
            SelectedSeverity = null;
            StartDate = null;
            EndDate = null;
            SearchText = string.Empty;
            _ = LoadIncidentsAsync();
        }

        private async Task DeleteIncidentAsync(int id)
        {
            try
            {
                var result = MessageBox.Show("Are you sure you want to delete this incident?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await _apiService.DeleteIncidentAsync(id);
                    await LoadIncidentsAsync();
                    MessageBox.Show("Incident deleted successfully!", "Success", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting incident: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task UpdateStatusAsync(string? status)
        {
            if (SelectedIncident == null || string.IsNullOrEmpty(status))
                return;

            try
            {
                string? analystName = null;
                
                // RESOLVED veya CLOSED durumlarında analist ismini sor
                if (status == "RESOLVED" || status == "CLOSED")
                {
                    var dialog = new Views.AnalystNameDialog();
                    if (dialog.ShowDialog() == true)
                    {
                        analystName = dialog.AnalystName;
                    }
                    else
                    {
                        // Kullanıcı iptal ettiyse işlemi durdur
                        return;
                    }
                }

                await _apiService.UpdateIncidentStatusAsync(SelectedIncident.IncidentId, status, analystName);
                await LoadIncidentsAsync();
                MessageBox.Show("Status updated successfully!", "Success", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating status: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetails(Incident? incident)
        {
            if (incident == null)
                return;

            var detailsWindow = new Views.IncidentDetailWindow(incident);
            detailsWindow.ShowDialog();
        }

        private void EditIncident(Incident? incident)
        {
            if (incident == null)
                return;

            var editViewModel = new EditIncidentViewModel(incident);
            var editWindow = new Views.EditIncidentWindow
            {
                DataContext = editViewModel
            };

            editViewModel.RequestClose += () => editWindow.Close();
            
            editWindow.ShowDialog();
            
            // Refresh list after edit
            _ = LoadIncidentsAsync();
        }

        private void SendEmail(Incident? incident)
        {
            if (incident == null)
                return;

            try
            {
                EmailHelper.SendIncidentEmail(incident);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening email client: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

