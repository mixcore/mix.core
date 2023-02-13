using Mix.Database.Entities.Account;
using Mix.Shared.Interfaces;

namespace Mix.Account
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<EntityRepository<MixCmsContext, MixCulture, int>>();
            services.AddScoped<EntityRepository<MixCmsAccountContext, MixUser, Guid>>();
            services.AddScoped<EntityRepository<MixCmsAccountContext, RefreshTokens, Guid>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
