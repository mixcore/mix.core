using Microsoft.Extensions.DependencyInjection;
using Mix.Cms.Lib.Controllers;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.NetCore;
using System;
using System.Linq;
using System.Reflection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var candidates = Assembly.GetExecutingAssembly()
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

        public static IServiceCollection AddGenerateApis(this IServiceCollection services)
        {
            services.AddGeneratedRestApi(Assembly.GetExecutingAssembly(), typeof(BaseRestApiController<,,>));
            services.AddSignalR();
            return services;
        }
    }
}