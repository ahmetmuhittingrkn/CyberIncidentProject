using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace CyberIncidentWPF
{
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            _ = TestBackendOnStartup();
        }

        private async Task TestBackendOnStartup()
        {
            try
            {
                StatusText.Text = "Backend bağlantısı test ediliyor...";
                
                var handler = new System.Net.Http.HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };
                
                using var client = new HttpClient(handler);
                client.Timeout = TimeSpan.FromSeconds(5);
                
                var response = await client.GetAsync("http://localhost:8080/api/incidents");
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    StatusText.Text = $"✅ Backend'e başarıyla bağlandı!\n\nVeri sayısı: {content.Length} karakter";
                    StatusText.Foreground = System.Windows.Media.Brushes.Green;
                    ErrorText.Text = $"İlk 200 karakter:\n{content.Substring(0, Math.Min(200, content.Length))}...";
                    ErrorText.Foreground = System.Windows.Media.Brushes.Green;
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    StatusText.Text = $"⚠️ Backend yanıt verdi ama hata: {response.StatusCode}";
                    StatusText.Foreground = System.Windows.Media.Brushes.Orange;
                    ErrorText.Text = $"Detay: {content}";
                }
            }
            catch (Exception ex)
            {
                StatusText.Text = "❌ Backend'e bağlanılamadı!";
                StatusText.Foreground = System.Windows.Media.Brushes.Red;
                ErrorText.Text = $"Hata Tipi: {ex.GetType().Name}\n\nMesaj: {ex.Message}\n\nInner: {ex.InnerException?.Message}";
            }
        }

        private async void TestBackend_Click(object sender, RoutedEventArgs e)
        {
            await TestBackendOnStartup();
        }

        private void OpenMainWindow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorText.Text = $"Ana pencere açılırken hata: {ex.Message}\n\nDetay: {ex.StackTrace}";
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

