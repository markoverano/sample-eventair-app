using GraphQL.Client.Http;
using Newtonsoft.Json;
using SampleEventsAirApp.Services;
using System.Net.Http.Headers;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQL;

namespace SampleEventsAirApp
{
    public class EventairGraphQLClient : IEventairGraphQLClient
    {
        private readonly GraphQLHttpClient _client;
        private readonly IEventairAuthService _tokenService;

        public EventairGraphQLClient(string hostname, string tenantId, IEventairAuthService tokenService)
        {
            _tokenService = tokenService;

            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(hostname),
            };

            _client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer());
            _client.HttpClient.DefaultRequestHeaders.Add("X-Tenant-Id", tenantId);
        }

        private async Task SetBearerTokenAsync()
        {
            var accessToken = await _tokenService.GetAccessTokenAsync();
            _client.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<dynamic> SendMutationAsync(string mutation, object variables)
        {
            await SetBearerTokenAsync();
            var request = new GraphQLRequest
            {
                Query = mutation,
                Variables = ConvertToGraphQLInputs(variables)
            };
            var response = await _client.SendMutationAsync<dynamic>(request);
            return response.Data;
        }

        public async Task<dynamic> SendQueryAsync(string query, object variables)
        {
            await SetBearerTokenAsync();
            var request = new GraphQLRequest
            {
                Query = query,
                Variables = ConvertToGraphQLInputs(variables)
            };
            var response = await _client.SendQueryAsync<dynamic>(request);
            return response.Data;
        }

        private static Inputs ConvertToGraphQLInputs(object variables)
        {
            var json = JsonConvert.SerializeObject(variables);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return new Inputs(dictionary);
        }
    }
}
