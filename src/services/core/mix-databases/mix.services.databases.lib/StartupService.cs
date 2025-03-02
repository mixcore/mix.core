using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Services.Databases.Lib.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Databases.Lib
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.TryAddScoped<MixDbDbContext>();
            builder.Services.TryAddScoped<MySqlMixDbDbContext>();
            builder.Services.TryAddScoped<SqliteMixDbDbContext>();
            builder.Services.TryAddScoped<UnitOfWorkInfo<MixDbDbContext>>();
            builder.Services.TryAddScoped<IMixPermissionService, MixUserPermissionService>();
            builder.Services.TryAddScoped<IMixMetadataService, MixMetadataService>();
            builder.Services.TryAddScoped<IMixUserDataService, MixUserDataService>();
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