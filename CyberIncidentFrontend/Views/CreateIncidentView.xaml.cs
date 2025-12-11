using System.Windows.Controls;
using CyberIncidentWPF.ViewModels;

namespace CyberIncidentWPF.Views
{
    public partial class CreateIncidentView : UserControl
    {
        public CreateIncidentView()
        {
            InitializeComponent();
            DataContext = new CreateIncidentViewModel();
        }
    }
}

