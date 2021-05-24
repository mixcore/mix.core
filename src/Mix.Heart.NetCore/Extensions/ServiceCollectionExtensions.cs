using Microsoft.Extensions.DependencyInjection;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Providers;
using Mix.Heart.RestFul.Conventions;
using System;
using System.Linq;
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
        
        public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var candidates = assembly
                .GetExportedTypes()
                .Where(m => m.BaseType?.Name == typeof(ViewModelBase<,,>).Name);
            var repositoryType = typeof(DefaultRepository<,,>);
            foreach (var candidate in candidates)
            {
                if (candidate.BaseType.IsGenericType
                    && candidate.BaseType.GenericTypeArguments.Length == repositoryType.GetGenericArguments().Length)
                {
                    Type[] types = candidate.BaseType.GenericTypeArguments;
                    services.AddScoped(
                        repositoryType.MakeGenericType(types)
                    );
                }
            }
            return services;
        }
    }
}