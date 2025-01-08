using System.Net.Http.Headers;

namespace SampleEventsAirApp.Services
{
    public class EventsAirService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventairAuthService _tokenService;

        public EventsAirService(HttpClient httpClient, IEventairAuthService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<string> ExecuteGraphQLQueryAsync(string query)
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestPayload = new
            {
                query = query
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.eventsair.com/graphql", requestPayload);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"GraphQL query failed: {response.StatusCode}, Details: {errorContent}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
