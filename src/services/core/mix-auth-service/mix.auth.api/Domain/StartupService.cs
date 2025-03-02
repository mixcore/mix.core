using Mix.Auth.Api.Domain.Subscribers;
using Mix.Database.Entities.Account;
using Mix.Shared.Interfaces;

namespace mix.auth.service.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<EntityRepository<MixCmsContext, MixCulture, int>>();
            builder.Services.AddScoped<EntityRepository<MixCmsAccountContext, MixUser, Guid>>();
            builder.Services.AddScoped<EntityRepository<MixCmsAccountContext, RefreshTokens, Guid>>();
            builder.Services.AddHostedService<MixAuthBackgroundTaskSubscriber>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}
