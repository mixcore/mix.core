using Mix.Database.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {

        private static void ApplyMigrations(this IServiceCollection services)
        {

            if (!GlobalConfigService.Instance.AppSettings.IsInit)
            {
                var mixDatabaseService = services.GetService<MixDatabaseService>();
                mixDatabaseService.InitMixCmsContextAsync().GetAwaiter();

                // TODO: Update cache service
                //MixCacheService.InitMixCacheContext();
            }
        }
    }
}