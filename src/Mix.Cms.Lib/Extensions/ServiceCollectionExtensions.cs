using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Controllers;
using Mix.Heart.NetCore;
using System.Reflection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddRepositories(Assembly.GetExecutingAssembly());
            return services;
        }

        public static IServiceCollection AddGenerateApis(this IServiceCollection services)
        {
            services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,>));
            return services;
        }
    }
}