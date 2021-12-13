using System.Reflection;
using Mix.Database.Entities.Account;
using Mix.Heart.Entities;
using Mix.Heart.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetAssembly(typeof(MixCmsContext));
            var cmsEntities = assembly
                .GetExportedTypes()
                .Where(
                myType =>
                myType.Namespace == typeof(MixCmsContext).Namespace
                && myType.IsClass && !myType.IsAbstract && myType.BaseType.IsAbstract
                && (
                    myType.IsSubclassOf(typeof(Entity))
                    || myType.IsSubclassOf(typeof(EntityBase<int>))
                    || myType.IsSubclassOf(typeof(EntityBase<Guid>)
                )));

            var accountEntities = assembly
                .GetExportedTypes()
                .Where(
                myType =>
                myType.Namespace == typeof(MixCmsAccountContext).Namespace
                && myType.IsClass
                && (
                    myType.IsSubclassOf(typeof(Entity))
                    || myType.IsSubclassOf(typeof(IEntity<string>))
                ));

            services.AddRepositories(cmsEntities, typeof(MixCmsContext));
            services.AddRepositories(accountEntities, typeof(MixCmsAccountContext));
            return services;
        }
    }
}
