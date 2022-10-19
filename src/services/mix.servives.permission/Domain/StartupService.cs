using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.Servives.Permission.Domain.Entities;
using Mix.Shared.Interfaces;
using Org.BouncyCastle.Crypto.Signers;

namespace Mix.Servives.Permission.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            var dbService = services.GetService<DatabaseService>();
            services.AddDbContext<PermissionDbContext>();
            var ct = new PermissionDbContext(dbService);
            var migrateions = ct.Database.GetPendingMigrations();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
