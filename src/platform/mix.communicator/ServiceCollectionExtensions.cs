using Mix.Communicator.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixCommunicators(this IServiceCollection services)
        {

            services.AddSingleton<FirebaseService>();
            services.AddScoped<EmailService>();
            return services;
        }
    }
}
