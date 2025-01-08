namespace SampleEventsAirApp.Services
{
    public interface IEventairAuthService
    {
        Task<string> GetAccessTokenAsync();
    }
}
