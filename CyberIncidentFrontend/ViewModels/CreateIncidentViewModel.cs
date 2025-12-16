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
    public class CreateIncidentViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _selectedType = "PHISHING";
        private string _selectedSeverity = "MEDIUM";
        private DateTime _incidentDate = DateTime.Now;
        private User? _selectedReporter;
        private bool _isSubmitting;
        private string _openedByAnalyst = string.Empty;
        private string _iocs = string.Empty;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string SelectedType
        {
            get => _selectedType;
            set => SetProperty(ref _selectedType, value);
        }

        public string SelectedSeverity
        {
            get => _selectedSeverity;
            set => SetProperty(ref _selectedSeverity, value);
        }

        public DateTime IncidentDate
        {
            get => _incidentDate;
            set => SetProperty(ref _incidentDate, value);
        }

        public User? SelectedReporter
        {
            get => _selectedReporter;
            set => SetProperty(ref _selectedReporter, value);
        }

        public bool IsSubmitting
        {
            get => _isSubmitting;
            set => SetProperty(ref _isSubmitting, value);
        }

        public string OpenedByAnalyst
        {
            get => _openedByAnalyst;
            set => SetProperty(ref _openedByAnalyst, value);
        }

        public string Iocs
        {
            get => _iocs;
            set => SetProperty(ref _iocs, value);
        }

        public ObservableCollection<string> IncidentTypes { get; }
        public ObservableCollection<string> SeverityLevels { get; }
        public ObservableCollection<User> Users { get; }

        public ICommand CreateIncidentCommand { get; }
        public ICommand ClearFormCommand { get; }

        public CreateIncidentViewModel()
        {
            // Singleton pattern - HttpClient socket tükenmesini önler
            _apiService = ApiServiceProvider.Instance;

            IncidentTypes = new ObservableCollection<string>
            {
                "PHISHING", "UNAUTHORIZED_ACCESS", "MALWARE", "DATA_BREACH",
                "DOS_ATTACK", "SOCIAL_ENGINEERING", "RANSOMWARE", "INSIDER_THREAT", "OTHER"
            };

            SeverityLevels = new ObservableCollection<string>
            {
                "LOW", "MEDIUM", "HIGH", "CRITICAL"
            };

            Users = new ObservableCollection<User>();

            CreateIncidentCommand = new RelayCommand(async _ => await CreateIncidentAsync(), _ => CanCreateIncident());
            ClearFormCommand = new RelayCommand(_ => ClearForm());

            // Load users
            _ = LoadUsersAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var users = await _apiService.GetUsersAsync();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                if (Users.Count > 0)
                {
                    SelectedReporter = Users[0];
                }
            }
            catch (Exception)
            {
                // Silently fail or log
            }
        }

        private bool CanCreateIncident()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   !string.IsNullOrWhiteSpace(Description) &&
                   !string.IsNullOrWhiteSpace(OpenedByAnalyst) &&
                   SelectedReporter != null &&
                   !IsSubmitting;
        }

        private async Task CreateIncidentAsync()
        {
            try
            {
                IsSubmitting = true;

                var incident = new Incident
                {
                    Title = Title,
                    Description = Description,
                    IncidentType = SelectedType,
                    SeverityLevel = SelectedSeverity,
                    IncidentDate = IncidentDate,
                    ReporterId = SelectedReporter?.UserId ?? 1,
                    OpenedByAnalyst = OpenedByAnalyst,
                    Iocs = string.IsNullOrWhiteSpace(Iocs) ? null : Iocs
                };

                var result = await _apiService.CreateIncidentAsync(incident);

                MessageBox.Show($"Incident created successfully!\nIncident ID: {result.IncidentId}", 
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating incident: {ex.Message}", "Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        private void ClearForm()
        {
            Title = string.Empty;
            Description = string.Empty;
            SelectedType = "PHISHING";
            SelectedSeverity = "MEDIUM";
            IncidentDate = DateTime.Now;
            OpenedByAnalyst = string.Empty;
            Iocs = string.Empty;
            if (Users.Count > 0) SelectedReporter = Users[0];
        }
    }
}

