using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        private static IServiceCollection AddMixDbContexts(this IServiceCollection services, Assembly executingAssembly, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>();
            services.AddDbContext<MixCmsContext>(ServiceLifetime.Transient);
            services.AddDbContext<MixCmsAccountContext>();
            return services;
        }
    }
}
