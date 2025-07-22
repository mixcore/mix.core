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
                using var serviceProvider = services.BuildServiceProvider();
                var mixDatabaseService = serviceProvider.GetRequiredService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContext();
            }
        }
    }
}