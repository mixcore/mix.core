using Microsoft.Extensions.Configuration;
using Mix.Quartz.Models;
using Mix.Quartz.Services;
using Mix.Shared;
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
            services.AddSingleton<IQuartzService, QuartzService>();
            services.AddHostedService<QuartzHostedService>();
            return services;
        }

        private static void AddSchedulerJobs(this IServiceCollection services)
        {
            var assemblies = MixAssemblyFinder.GetMixAssemblies();

            foreach (var assembly in assemblies)
            {
                var mixJobs = assembly
                    .GetExportedTypes()
                    .Where(m => m.BaseType == typeof(MixJobBase));

                var method = typeof(ServiceCollectionServiceExtensions)
                    .GetMethods()
                    .First(m => m.IsGenericMethodDefinition
                        && m.Name == nameof(ServiceCollectionServiceExtensions.AddSingleton)
                        && m.GetGenericArguments().Length == 2);

                foreach (var job in mixJobs)
                {
                    MethodInfo generic = method.MakeGenericMethod(typeof(MixJobBase), job);
                    generic.Invoke(null, new object[] { services });
                }
            }
        }

    }
}