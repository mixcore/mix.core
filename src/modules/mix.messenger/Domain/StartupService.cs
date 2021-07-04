using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
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
                  })
                  .AddMessagePackProtocol();
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PortalHub>("/portalHub");
                endpoints.MapHub<EditFileHub>("/editFileHub");
            });
        }
    }
}
