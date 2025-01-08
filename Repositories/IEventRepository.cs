using SampleEventsAirApp.DTOs;

namespace SampleEventsAirApp.Repositories
{
    public interface IEventRepository
    {
        Task<string> CreateEventAsync(CreateEventRequest request);
    }
}
