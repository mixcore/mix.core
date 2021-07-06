using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Entities;
using System;
using System.Linq;
using System.Reflection;
using Mix.Heart.Extensions;

namespace Mix.Database.Extenstions
{
    public static class ServiceCollectionExtensions
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
