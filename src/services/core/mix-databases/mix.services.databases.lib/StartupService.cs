using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.MixDb;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Databases.Lib
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<MixDbDbContext>();
            services.TryAddScoped<MySqlMixDbDbContext>();
            services.TryAddScoped<SqliteMixDbDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixDbDbContext>>();
            services.TryAddScoped<IMixPermissionService, MixUserPermissionService>();
            services.TryAddScoped<IMixMetadataService, MixMetadataService>();
            services.TryAddScoped<IMixUserDataService, MixUserDataService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixDbDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}