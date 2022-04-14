using Microsoft.Extensions.Configuration;
using Mix.Quartz.Models;
using Mix.Quartz.Services;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixQuartzServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddSchedulerJobs();
            services.AddSingleton<IJobFactory, SingletonJobFactory>();
            services.AddSingleton<QuartzService>();
            services.AddHostedService<QuartzHostedService>();
            return services;
        }

        private static void AddSchedulerJobs(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var mixJobs = assembly
                .GetExportedTypes()
                .Where(m => m.BaseType.Name == typeof(MixJobBase).Name);
            var applyGenericMethod = typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .First(m => m.IsGenericMethodDefinition
                    && m.Name == nameof(ServiceCollectionServiceExtensions.AddSingleton)
                    && m.GetGenericArguments().Length == 2
                    );
            foreach (var job in mixJobs)
            {
                MethodInfo generic = applyGenericMethod.MakeGenericMethod(typeof(MixJobBase), job);
                generic.Invoke(null, new object[] { services });
            }
        }

    }
}