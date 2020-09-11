using Microsoft.Extensions.DependencyInjection;
using Mix.Heart.Providers;
using Mix.Heart.RestFul.Conventions;
using System;
using System.Reflection;

namespace Mix.Heart.NetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGeneratedRestApi(this IServiceCollection services, Assembly assembly, Type baseType = null)
        {
            services.
                AddMvc(o => o.Conventions.Add(
                    new GenericControllerRouteConvention()
                )).
                ConfigureApplicationPartManager(m =>
                    m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider(assembly, baseType)
                ));
            return services;
        }
    }
}
