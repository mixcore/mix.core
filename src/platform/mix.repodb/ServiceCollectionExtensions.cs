﻿using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.Services;
using Mix.Shared.Models.Configurations;
using RepoDb;
using RepoDb.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixRepoDb(this IServiceCollection services, GlobalSettingsModel globalConfig)
        {
            services.TryAddScoped<ICache, MemoryCache>();

            services.TryAddScoped<MixRepoDbRepository>();
            services.TryAddScoped<IMixDbDataService, MixDbDataService>();
            services.TryAddScoped<IMixDbService, MixDbService>();

            return services;
        }
    }
}
