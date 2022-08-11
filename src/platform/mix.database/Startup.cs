using Mix.Database.Entities.Account;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.Runtime;
using Mix.Shared.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MixDbStartup
    {
        public static IServiceCollection AddMixDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<MixCmsAccountContext>();
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixCmsAccountContext>();
            services.AddDbContext<AuditLogDbContext>();
            
            return services;
        }
    }
}
