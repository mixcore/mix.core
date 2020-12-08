using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Controllers;
using Mix.Heart.NetCore;
using System.Reflection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMyGraphQL(this IServiceCollection services)
        {
            services.AddSignalR();
            return services;
        }

        public static IServiceCollection AddGenerateApis(this IServiceCollection services)
        {
            services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,>));
            //services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,,>));
            //services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,,,>));

            services.AddSignalR();
            return services;
        }
    }
}
