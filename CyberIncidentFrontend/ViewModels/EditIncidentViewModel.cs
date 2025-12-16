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
    public class EditIncidentViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        private readonly Incident _originalIncident;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _selectedType = "PHISHING";
        private string _selectedSeverity = "MEDIUM";
        private DateTime _incidentDate = DateTime.Now;
        private User? _selectedReporter;
        private bool _isSubmitting;
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

        public string Iocs
        {
            get => _iocs;
            set => SetProperty(ref _iocs, value);
        }

        public ObservableCollection<string> IncidentTypes { get; }
        public ObservableCollection<string> SeverityLevels { get; }
        public ObservableCollection<User> Users { get; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action? RequestClose;

        public EditIncidentViewModel(Incident incident)
        {
            _apiService = ApiServiceProvider.Instance;
            _originalIncident = incident;

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

            SaveCommand = new RelayCommand(async _ => await SaveAsync(), _ => CanSave());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            // Initialize fields
            Title = incident.Title;
            Description = incident.Description;
            SelectedType = incident.IncidentType;
            SelectedSeverity = incident.SeverityLevel;
            IncidentDate = incident.IncidentDate;
            Iocs = incident.Iocs ?? string.Empty;

            // Load users and select the reporter
            _ = LoadUsersAsync(incident.ReporterId);
        }

        private async Task LoadUsersAsync(int reporterId)
        {
            try
            {
                var users = await _apiService.GetUsersAsync();
                Users.Clear();
                foreach (var user in users)
                {
                    Users.Add(user);
                }

                SelectedReporter = Users.FirstOrDefault(u => u.UserId == reporterId);
                if (SelectedReporter == null && Users.Any())
                {
                    SelectedReporter = Users.First();
                }
            }
            catch (Exception)
            {
                // Silently fail
            }
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Title) &&
                   !string.IsNullOrWhiteSpace(Description) &&
                   SelectedReporter != null &&
                   !IsSubmitting;
        }

        private async Task SaveAsync()
        {
            try
            {
                IsSubmitting = true;

                var updatedIncident = new Incident
                {
                    IncidentId = _originalIncident.IncidentId,
                    Title = Title,
                    Description = Description,
                    IncidentType = SelectedType,
                    SeverityLevel = SelectedSeverity,
                    IncidentDate = IncidentDate,
                    ReporterId = SelectedReporter?.UserId ?? 1,
                    Status = _originalIncident.Status, // Preserve status
                    Iocs = string.IsNullOrWhiteSpace(Iocs) ? null : Iocs
                };

                await _apiService.UpdateIncidentAsync(_originalIncident.IncidentId, updatedIncident);

                MessageBox.Show("Incident updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating incident: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSubmitting = false;
            }
        }
    }
}
