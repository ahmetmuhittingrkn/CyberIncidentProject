using System.Windows;

namespace CyberIncidentWPF.Views
{
    public partial class AnalystNameDialog : Window
    {
        public string AnalystName { get; private set; } = string.Empty;

        public AnalystNameDialog()
        {
            InitializeComponent();
            AnalystNameTextBox.Focus();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AnalystNameTextBox.Text))
            {
                MessageBox.Show("Please enter your name.", "Validation Error", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AnalystName = AnalystNameTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
