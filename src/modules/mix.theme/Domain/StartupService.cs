using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mix.Lib.Interfaces;
using Mix.Theme.Domain.Services;

namespace Mix.Theme.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddScoped<InitCmsService>();
            services.AddScoped<ImportSiteService>();
        }

        public void UseApps(IApplicationBuilder app, bool isDevelop)
        {
        }
    }
}
