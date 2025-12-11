using System.Windows;

namespace CyberIncidentWPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Global exception handling
            this.DispatcherUnhandledException += (s, args) =>
            {
                MessageBox.Show($"An unexpected error occurred:\n\n{args.Exception.Message}", 
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };
        }
    }
}

