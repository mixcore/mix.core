using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using mix.library.Modules;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace mix.library.Extensions
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddMixServices(this IServiceCollection services)
        {
            var assemblies = GetMixAssemblies();
            var startupServices = assemblies.SelectMany(
                                        assembly => assembly.GetExportedTypes()
                                            .Where(IsStartupService));
            foreach (var startup in startupServices)
            {
                ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                var instance = classConstructor.Invoke(Array.Empty<object>());
                startup.GetMethod("AddServices").Invoke(instance, new object[] { services });
            }
            return services;
        }

        public static IApplicationBuilder UseMixApps(this IApplicationBuilder app)
        {
            var assemblies = GetMixAssemblies();
            var startupServices = assemblies.SelectMany(
                                        assembly => assembly.GetExportedTypes()
                                            .Where(IsStartupService));
            foreach (var startup in startupServices)
            {
                ConstructorInfo classConstructor = startup.GetConstructor(Array.Empty<Type>());
                var instance = classConstructor.Invoke(Array.Empty<object>());
                startup.GetMethod("UseApps").Invoke(instance, new object[] { app });
            }
            return app;
        }

        internal static bool IsStartupService(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return
                typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IStartupService).IsAssignableFrom(type);
        }


        private static Assembly[] GetMixAssemblies()
        {
            var assemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                                .Where(x => AssemblyName.GetAssemblyName(x).FullName.StartsWith("mix."))
                                .Select(x => Assembly.Load(AssemblyName.GetAssemblyName(x)));
            return assemblies.ToArray();
        }
    }
}
