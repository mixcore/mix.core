using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Middlewares;
using Mix.Shared.Interfaces;
using Mix.Universal.Lib.Entities;

namespace Mix.Universal.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MixUniversalDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixUniversalDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixUniversalDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
