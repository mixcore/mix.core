using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Middlewares;
using Mix.Shared.Interfaces;
using Mix.Universal.Lib.Entities;
using Mix.Universal.Lib.Middlewares;

namespace Mix.Universal.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MixUniversalDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixUniversalDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseMiddleware<MixUniversalUnitOfWorkMiddleware>();
        }
    }
}
