using SampleEventsAirApp.DTOs;

namespace SampleEventsAirApp.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventairGraphQLClient _client;
        private readonly string _tenantId;

        public EventRepository(IEventairGraphQLClient client, string tenantId)
        {
            _client = client;
            _tenantId = tenantId;
        }

        public async Task<string> CreateEventAsync(CreateEventRequest request)
        {
            var query = @"
            mutation CreateEvent($input: CreateEventInput!) {
                createEvent(input: $input) {
                    event {
                        id
                        name
                        alias
                        startDate
                        endDate
                        timeZone
                        currency
                        venue {
                            name
                            city
                            state
                            country
                        }
                        uniqueCode
                        options {
                            ceCourses
                            aircastStreaming
                            mode3D
                            remoteAttendee
                        }
                    }
                }
            }";

            var variables = new
            {
                input = new
                {
                    tenantId = _tenantId,
                    name = request.Name,
                    type = request.Type,
                    alias = request.Alias,
                    startDate = request.StartDate.ToString("yyyy-MM-dd"),
                    endDate = request.EndDate.ToString("yyyy-MM-dd"),
                    timeZone = request.TimeZone,
                    currency = request.Currency,
                    enableMultiCurrency = request.EnableMultiCurrency,
                    cloneFrom = request.CloneFrom,
                    eventGroup = request.EventGroup,
                    venue = new
                    {
                        name = request.Venue.Name,
                        city = request.Venue.City,
                        state = request.Venue.State,
                        country = request.Venue.Country
                    },
                    uniqueCode = request.UniqueCode,
                    contactStore = request.ContactStore,
                    eventLogoUrl = request.EventLogoUrl,
                    options = new
                    {
                        ceCourses = request.Options.HasCECourses,
                        aircastStreaming = request.Options.HasAIRCastStreaming,
                        mode3D = request.Options.Has3DMode,
                        remoteAttendee = request.Options.HasRemoteAttendeeMode
                    }
                }
            };

            var response = await _client.SendMutationAsync(query, variables);
            return response.createEvent.eventid.id;
        }
    }
}
