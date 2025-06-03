using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.RepoDb.Repositories;
using Mix.Scylladb.Repositories;
using Mix.Service.Services;
using Mix.Shared.Models.Configurations;
using RepoDb;
using RepoDb.Interfaces;

namespace Mix.Mixdb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixDbServices(this IServiceCollection services, GlobalSettingsModel globalConfig)
        {
            services.TryAddScoped<ICache, MemoryCache>();
            services.TryAddScoped<IMixDbInfoService, MixDbInfoService>();
            services.TryAddScoped<IMixDbDataParser, MixDbDataParser>();
            services.TryAddSingleton<ScylladbRepository>();

            // Register concrete implementations
            services.TryAddTransient<RepodbDataService>();
            services.TryAddScoped<ScylladbDataService>();

            // Register factory
            services.TryAddSingleton<IMixDbDataServiceFactory, MixDbDataServiceFactory>();

            services.TryAddScoped<IMixdbStructure, MixdbStructureService>();
            services.AddScoped<IMixdbStructureService, RepodbStructureService>();
            services.AddScoped<IMixdbStructureService, ScylladbStructureService>();

            return services;
        }
    }
}
