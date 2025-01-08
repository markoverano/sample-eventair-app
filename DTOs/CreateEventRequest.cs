namespace SampleEventsAirApp.DTOs
{
    public class CreateEventRequest
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Alias { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TimeZone { get; set; }
        public string Currency { get; set; }
        public bool EnableMultiCurrency { get; set; }
        public string? CloneFrom { get; set; }
        public string? EventGroup { get; set; }
        public VenueInfo Venue { get; set; }
        public string UniqueCode { get; set; }
        public string? ContactStore { get; set; }
        public string? EventLogoUrl { get; set; }
        public EventOptions Options { get; set; }
    }

    public class VenueInfo
    {
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }

    public class EventOptions
    {
        public bool HasCECourses { get; set; }
        public bool HasAIRCastStreaming { get; set; }
        public bool Has3DMode { get; set; }
        public bool HasRemoteAttendeeMode { get; set; }
    }
}
