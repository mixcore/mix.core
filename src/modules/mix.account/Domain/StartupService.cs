using Mix.Database.Entities.Account;
using Mix.Lib.Interfaces;

namespace Mix.Account.Domain
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

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
