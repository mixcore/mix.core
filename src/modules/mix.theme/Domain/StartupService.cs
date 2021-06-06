using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;

namespace Mix.Theme.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<InitCmsService>();
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
        {
        }
    }
}
