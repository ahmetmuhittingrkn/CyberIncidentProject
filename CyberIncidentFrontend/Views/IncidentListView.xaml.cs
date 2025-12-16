using System.Windows;
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
            
            // ContextMenu'nun DataContext'ini ayarla
            Loaded += IncidentListView_Loaded;
        }

        private void IncidentListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (StatusContextMenu != null && DataContext != null)
            {
                StatusContextMenu.DataContext = DataContext;
            }
        }

        private void UpdateStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusContextMenu != null && UpdateStatusButton != null)
            {
                // ContextMenu'nun DataContext'ini güncelle
                if (StatusContextMenu.DataContext == null && DataContext != null)
                {
                    StatusContextMenu.DataContext = DataContext;
                }
                
                StatusContextMenu.PlacementTarget = UpdateStatusButton;
                StatusContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                StatusContextMenu.HorizontalOffset = 0;
                StatusContextMenu.VerticalOffset = 5;
                StatusContextMenu.IsOpen = true;
            }
        }
    }
}

