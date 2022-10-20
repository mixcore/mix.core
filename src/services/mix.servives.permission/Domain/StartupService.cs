using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Permission.Domain.Entities;
using Mix.Shared.Interfaces;
using Mix.Shared.Services;
using Org.BouncyCastle.Crypto.Signers;

namespace Mix.Services.Permission.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<PermissionDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<PermissionDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<PermissionDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
