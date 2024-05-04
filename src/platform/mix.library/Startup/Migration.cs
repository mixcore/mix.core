using Mix.Database.Services;
using Mix.Shared.Models.Configurations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        public static void ApplyMigrations(this IServiceCollection services, GlobalSettingsModel options)
        {
            if (!options.IsInit)
            {
                var mixDatabaseService = services.GetService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContext();

            }
        }
    }
}