using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Services;
using Mix.Shared.Models.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        private static void ApplyMigrations(this IServiceCollection services, GlobalSettingsModel options)
        {
            if (!options.IsInit)
            {
                var mixDatabaseService = services.GetService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContext();

            }
        }
    }
}