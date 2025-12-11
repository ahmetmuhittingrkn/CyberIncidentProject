using System.Windows;
using CyberIncidentWPF.ViewModels;

namespace CyberIncidentWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}

