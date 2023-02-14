using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Communicator.Services;
using Mix.Quartz.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixCommunicators(this IServiceCollection services)
        {
            services.TryAddSingleton<FirebaseService>();
            services.TryAddScoped<EmailService>();
            return services;
        }
    }
}
