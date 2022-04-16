using Mix.Database.Entities.Account;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MixDbStartup
    {
        public static IServiceCollection AddMixDbContexts(this IServiceCollection services)
        {
            services.AddDbContext<MixCmsAccountContext>();
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixCmsAccountContext>();
            return services;
        }
    }
}
