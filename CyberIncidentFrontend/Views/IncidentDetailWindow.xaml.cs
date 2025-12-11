using System.Windows;
using CyberIncidentWPF.Models;

namespace CyberIncidentWPF.Views
{
    public partial class IncidentDetailWindow : Window
    {
        public IncidentDetailWindow(Incident incident)
        {
            InitializeComponent();
            DataContext = incident;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

