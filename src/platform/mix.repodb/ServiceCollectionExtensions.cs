using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.RepoDb.Repositories;
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
            services.TryAddScoped<MixDbService>();
            services.TryAddScoped<MixRepoDbRepository>();
            return services;
        }
    }
}
