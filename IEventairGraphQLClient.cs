namespace SampleEventsAirApp
{
    public interface IEventairGraphQLClient
    {
        public Task<dynamic> SendMutationAsync(string mutation, object variables);
        public Task<dynamic> SendQueryAsync(string query, object variables);
    }
}
