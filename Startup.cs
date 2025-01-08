using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleEventsAirApp.DTOs;
using SampleEventsAirApp.Repositories;
using SampleEventsAirApp.Services;
using System.Text;

namespace SampleEventsAirApp
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<EventsAirSettings>(_configuration.GetSection("EventsAir"));
            services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<EventsAirSettings>>().Value);


            services.AddScoped<IEventairGraphQLClient>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var hostname = configuration.GetValue<string>("EventsAir:Api:BaseUrl");
                var tenantId = configuration.GetValue<string>("EventsAir:Auth:TenantId");
                var authService = sp.GetRequiredService<IEventairAuthService>();

                return new EventairGraphQLClient(hostname, tenantId, authService);
            });

            services.AddSwaggerGen();

            services.AddScoped<IEventairAuthService, EventairAuthService>();

            services.AddScoped<IEventRepository>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var tenantId = configuration.GetValue<string>("EventsAir:Auth:TenantId");
                var graphQLClient = sp.GetRequiredService<IEventairGraphQLClient>();

                return new EventRepository(graphQLClient, tenantId);
            });

            services.AddScoped<IEventService, EventService>();

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                          p => p.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod());
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCors("AllowAll");

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Events Air API"));

            app.UseDeveloperExceptionPage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}