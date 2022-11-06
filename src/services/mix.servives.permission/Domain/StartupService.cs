using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Permission.Domain.Entities;
using Mix.Services.Permission.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Permission.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<PermissionDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<PermissionDbContext>>();
            services.TryAddScoped<MixPermissionService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PermissionDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
