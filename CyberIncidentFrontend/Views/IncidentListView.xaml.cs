using System.Windows.Controls;
using CyberIncidentWPF.ViewModels;

namespace CyberIncidentWPF.Views
{
    public partial class IncidentListView : UserControl
    {
        public IncidentListView()
        {
            InitializeComponent();
            DataContext = new IncidentListViewModel();
        }
    }
}

