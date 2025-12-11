using System.Windows.Controls;
using CyberIncidentWPF.ViewModels;

namespace CyberIncidentWPF.Views
{
    public partial class AnalyticsView : UserControl
    {
        public AnalyticsView()
        {
            InitializeComponent();
            DataContext = new AnalyticsViewModel();
        }
    }
}

