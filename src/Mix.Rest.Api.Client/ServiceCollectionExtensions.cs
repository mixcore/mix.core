using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Extensions;
using Mix.Heart.NetCore;
using System.Reflection;

namespace Mix.Rest.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestClientServices(this IServiceCollection services)
        {
            services.AddRepositories(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}