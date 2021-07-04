using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
using Mix.SignalR.Constants;
using Mix.SignalR.Hubs;

namespace Mix.Messenger.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddSignalR()
                  .AddJsonProtocol(options =>
                  {
                      options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                  });
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>(HubEndpoints.PortalHub);
                endpoints.MapHub<EditFileHub>(HubEndpoints.EditFileHub);
            });
        }
    }
}
