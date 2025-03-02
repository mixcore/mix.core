using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Lib.Interfaces;
using Mix.Shared.Interfaces;

namespace Mix.Messenger.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            string azureConnectionString = builder.Configuration.GetSection("Azure")["SignalRConnectionString"];
            string redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionStrings"];
            builder.AddMixSignalR(azureConnectionString, redisConnectionString);
            builder.Services.AddMixCommunicators();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            //app.UseMixSignalRApp();
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.UseMixSignalRApp();
        }
    }
}
