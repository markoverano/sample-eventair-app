using SampleEventsAirApp.DTOs;

namespace SampleEventsAirApp.Services
{
    public interface IEventService
    {
        Task<string> CreateEventAsync(CreateEventRequest request);
    }
}
