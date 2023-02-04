using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Databases.Lib
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<MixDbDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixDbDbContext>>();
            services.TryAddScoped<MixServiceDatabaseDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixServiceDatabaseDbContext>>();
            services.TryAddScoped<MixPermissionService>();
            services.TryAddScoped<MixMetadataService>();
            services.TryAddScoped<MixUserDataService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixDbDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixServiceDatabaseDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
