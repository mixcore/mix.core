using Mix.Lib.Publishers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddGeneratedPublisher(this IServiceCollection services)
        {
            List<Type> candidates = GetCandidatesByAttributeType(MixAssemblies, typeof(GeneratePublisherAttribute));
            var publisher = typeof(MixPublisher<>);
            var method = typeof(ServiceCollectionHostedServiceExtensions).GetMethods()
                .Where(m => m.Name == "AddHostedService" && m.IsGenericMethod && m.GetParameters().Count() == 1);
            foreach (var candidate in candidates)
            {
                var genericType = publisher.MakeGenericType(new[] { candidate.UnderlyingSystemType });
                var genericMethod = method.First().MakeGenericMethod(genericType);
                genericMethod.Invoke(services, new[] { services });
            }
            return services;
        }


    }
}