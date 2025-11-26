using Mix.MCP.Lib.Extensions;
using Mix.Shared.Interfaces;

namespace mix.mcp.api
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.AddMCPServices();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.MapMCPEndpoints(isDevelop);
        }
    }
}
