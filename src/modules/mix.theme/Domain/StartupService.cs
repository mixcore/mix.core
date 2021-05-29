using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;

namespace Mix.Theme.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services)
        {
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
        {
        }
    }
}
