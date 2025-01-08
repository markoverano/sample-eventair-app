namespace SampleEventsAirApp.DTOs
{
    public class EventsAirSettings
    {
        public AuthConfig Auth { get; set; }
        public ApiConfig Api { get; set; }

        public class AuthConfig
        {
            public string TenantId { get; set; }
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Scope { get; set; }
        }

        public class ApiConfig
        {
            public string BaseUrl { get; set; }
        }
    }
}
