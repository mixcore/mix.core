using Mix.Services.Graphql.Lib;
using Mix.Shared.Interfaces;

namespace Mix.Services.Graphql.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddMixGraphQL();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            //app.UseEndpoints(enpoints => enpoints.UseMixGraphQL());
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.UseMixGraphQL();
        }
    }
}
