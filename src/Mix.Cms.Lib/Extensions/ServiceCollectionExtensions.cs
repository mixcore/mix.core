using Microsoft.Extensions.DependencyInjection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }
    }
}
