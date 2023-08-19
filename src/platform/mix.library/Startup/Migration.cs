using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        private static void ApplyMigrations(this IServiceCollection services, Mix.Shared.Models.Configurations.GlobalConfigurations options)
        {
            if (!options.IsInit)
            {
                var mixDatabaseService = services.GetService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContext();

            }
        }
    }
}