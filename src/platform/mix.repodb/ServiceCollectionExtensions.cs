using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Heart.Enums;
using Mix.RepoDb.Services;
using RepoDb;
using RepoDb.Interfaces;

namespace Mix.RepoDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixDbRepository(this IServiceCollection services)
        {
            services.AddScoped<ICache, MemoryCache>();
            services.AddScoped<MixDbService>();
            return services;
        }
    }
}
