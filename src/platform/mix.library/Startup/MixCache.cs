using Microsoft.Extensions.Configuration;
using Mix.Heart.Entities.Cache;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddMixCache(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {

            services.AddSingleton<MixCacheDbContext>();
            services.AddSingleton<EntityRepository<MixCacheDbContext, MixCache, string>>();
            services.AddSingleton<MixCacheService>();

            return services;
        }
    }
}
