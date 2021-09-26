using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mix.Account.Domain.Services;
using Mix.Lib.Interfaces;

namespace Mix.Account
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<MixGrpcAccountService>();
            });
        }
    }
}
