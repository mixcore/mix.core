using Microsoft.Extensions.Configuration;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Shared.Models.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        public static void ApplyMigrations(this IServiceCollection services, IConfiguration configuration, GlobalSettingsModel options)
        {
            if (!configuration.IsInit())
            {
                var mixDatabaseService = services.GetService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContext();

            }
        }
    }
}