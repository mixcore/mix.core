using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.RepoDb.Repositories;
using Mix.Scylladb.Repositories;
using Mix.Shared.Models.Configurations;
using RepoDb;
using RepoDb.Interfaces;

namespace Mix.Mixdb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRepoDb(this IServiceCollection services, GlobalSettingsModel globalConfig)
        {
            services.TryAddScoped<ICache, MemoryCache>();

            services.AddScoped<IMixDbDataService, RepodbDataService>();
            services.TryAddSingleton<ScylladbRepository>();
            services.AddScoped<IMixDbDataService, ScylladbDataService>();
            services.TryAddScoped<IMixdbStructure, MixdbStructureService>();
            services.AddScoped<IMixdbStructureService, RepodbStructureService>();
            services.AddScoped<IMixdbStructureService, ScylladbStructureService>();
            services.TryAddScoped<MixDbDataServiceFactory>();

            return services;
        }
    }
}
