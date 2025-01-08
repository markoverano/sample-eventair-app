using Microsoft.AspNetCore.Mvc;
using SampleEventsAirApp.DTOs;
using SampleEventsAirApp.Services;

namespace SampleEventsAirApp.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateEvent([FromBody] CreateEventRequest request)
        {
            try
            {
                var eventId = await _eventService.CreateEventAsync(request);

                return Ok(new { id = eventId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to create event", message = ex.Message });
            }
        }
    }
}
