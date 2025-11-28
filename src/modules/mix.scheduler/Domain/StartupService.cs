using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.Lib.Interfaces;
using Mix.Shared.Interfaces;

namespace Mix.Scheduler.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.AddMixQuartzServices();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
