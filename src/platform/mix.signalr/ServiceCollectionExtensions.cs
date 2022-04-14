namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixSignalR(this IServiceCollection services)
        {

            services.AddSignalR()
                   .AddJsonProtocol();
            return services;
        }
    }
}
