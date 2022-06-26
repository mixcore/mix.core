using Mix.Database.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        private static void ApplyMigrations(this IServiceCollection services)
        {

            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                var mixDatabaseService = services.GetService<DatabaseService>();
                mixDatabaseService.UpdateMixCmsContextAsync().GetAwaiter();
            }
        }
    }
}