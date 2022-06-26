using Mix.RepoDb.Services;
using RepoDb;
using RepoDb.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRepoDb(this IServiceCollection services)
        {
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<MixDbService>();
            return services;
        }
    }
}
