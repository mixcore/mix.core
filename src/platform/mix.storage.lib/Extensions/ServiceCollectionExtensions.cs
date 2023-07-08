using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Storage.Lib.Services;
using Mix.Storage.Lib.Subscribers;

namespace Mix.Storage.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixStorage(this IServiceCollection services)
        {
            services.TryAddScoped<MixStorageService>();
            services.AddHostedService<StorageBackgroundTaskSubscriber>();
            return services;
        }
    }
}
