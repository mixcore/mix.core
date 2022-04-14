using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MixDbStartup
    {
        public static IServiceCollection AddMixDbContexts(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {
            services.AddDbContext<MixCmsAccountContext>();
            services.AddDbContext<MixCmsContext>();
            services.AddDbContext<MixCmsAccountContext>();
            return services;
        }
    }
}
