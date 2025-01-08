using SampleEventsAirApp.DTOs;
using SampleEventsAirApp.Repositories;

namespace SampleEventsAirApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<string> CreateEventAsync(CreateEventRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                throw new ArgumentException("Event name is required");

            if (request.EndDate <= request.StartDate)
                throw new ArgumentException("End date must be after start date");

            if (string.IsNullOrEmpty(request.TimeZone))
                throw new ArgumentException("Time zone is required");

            if (string.IsNullOrEmpty(request.Currency))
                throw new ArgumentException("Currency is required");

            return await _eventRepository.CreateEventAsync(request);
        }
    }
}
