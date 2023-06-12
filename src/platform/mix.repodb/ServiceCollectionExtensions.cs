using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Publishers;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;
using Mix.RepoDb.Subscribers;
using Mix.Shared.Services;
using RepoDb;
using RepoDb.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRepoDb(this IServiceCollection services)
        {
            services.TryAddScoped<ICache, MemoryCache>();
            services.TryAddScoped<IMixDbDataService, MixDbDataService>();
            services.TryAddScoped<MixRepoDbRepository>();
            services.TryAddScoped<IMixDbService, MixDbService>();
            services.AddHostedService<MixRepoDbPublisher>();
            services.AddHostedService<MixRepoDbSubscriber>();

            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                var mixDbService = services.GetService<IMixDbService>();
                mixDbService.MigrateSystemDatabases().GetAwaiter().GetResult();
            }
            return services;
        }
    }
}
