using System;

namespace CyberIncidentWPF.Services
{
    /// <summary>
    /// ApiService için Singleton sağlayıcı.
    /// HttpClient socket tükenmesini önlemek için tek instance kullanılır.
    /// Thread-safe lazy initialization pattern.
    /// </summary>
    public static class ApiServiceProvider
    {
        private static readonly Lazy<ApiService> _instance = 
            new Lazy<ApiService>(() => new ApiService(), isThreadSafe: true);

        /// <summary>
        /// Singleton ApiService instance'ı döndürür.
        /// Tüm ViewModel'ler bu instance'ı kullanmalıdır.
        /// </summary>
        public static ApiService Instance => _instance.Value;

        /// <summary>
        /// Test amaçlı instance'ı sıfırlar.
        /// Production kodda KULLANILMAMALI!
        /// </summary>
        internal static void Reset()
        {
            // Lazy<T> reset edilemez, bu metot sadece test için placeholder
            throw new NotSupportedException("ApiService instance cannot be reset in production.");
        }
    }
}

