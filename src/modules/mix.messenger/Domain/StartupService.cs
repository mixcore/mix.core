using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Messenger.Domain.Services;
using Mix.Shared.Interfaces;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;

namespace Mix.Messenger.Domain
{
    public class StartupService: IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSignalR()
            //      .AddJsonProtocol(options =>
            //      {
            //          options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            //      });
            services.AddSingleton<FirebaseService>();
            services.AddScoped<EmailService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapHub<PortalHub>(HubEndpoints.PortalHub);
            //    endpoints.MapHub<EditFileHub>(HubEndpoints.EditFileHub);
            //});
        }

    }
}
