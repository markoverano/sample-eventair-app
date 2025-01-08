using SampleEventsAirApp.DTOs;

namespace SampleEventsAirApp.Services
{
    public class EventairAuthService : IEventairAuthService
    {
        private readonly EventsAirSettings _config;
        private readonly HttpClient _httpClient;
        private string _cachedToken;
        private DateTime _tokenExpiration;

        public EventairAuthService(EventsAirSettings config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://login.microsoftonline.com")
            };
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_cachedToken) && DateTime.UtcNow < _tokenExpiration)
            {
                return _cachedToken;
            }

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", _config.Auth.Scope),
                new KeyValuePair<string, string>("client_id", _config.Auth.ClientId),
                new KeyValuePair<string, string>("client_secret", _config.Auth.ClientSecret)
            });

            var response = await _httpClient.PostAsync(
                $"/{_config.Auth.TenantId}/oauth2/v2.0/token",
                content
            );

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Authentication failed: {response.StatusCode}, Details: {errorContent}");
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
            _cachedToken = tokenResponse.AccessToken;
            _tokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 300);

            return _cachedToken;
        }
    }

}
