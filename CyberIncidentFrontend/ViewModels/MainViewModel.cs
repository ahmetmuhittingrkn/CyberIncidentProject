using System.Windows.Input;
using CyberIncidentWPF.Helpers;

namespace CyberIncidentWPF.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowIncidentListCommand { get; }
        public ICommand ShowCreateIncidentCommand { get; }
        public ICommand ShowAnalyticsCommand { get; }

        public MainViewModel()
        {
            ShowIncidentListCommand = new RelayCommand(_ => ShowIncidentList());
            ShowCreateIncidentCommand = new RelayCommand(_ => ShowCreateIncident());
            ShowAnalyticsCommand = new RelayCommand(_ => ShowAnalytics());

            // Show incident list by default
            ShowIncidentList();
        }

        private void ShowIncidentList()
        {
            CurrentView = new IncidentListViewModel();
        }

        private void ShowCreateIncident()
        {
            CurrentView = new CreateIncidentViewModel();
        }

        private void ShowAnalytics()
        {
            CurrentView = new AnalyticsViewModel();
        }
    }
}

