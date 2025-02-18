using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.Database.Entities.Settings;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Services;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IHostApplicationBuilder AddConfigurations(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            using var dbContext = GetSettingDbContext(builder.Configuration);
            {

                if (dbContext.Database.GetPendingMigrations().Any())
                {
                    dbContext.Database.Migrate();
                }

                var settings = dbContext.MixGlobalSetting.Where(m => m.ServiceName == EnvironmentService.ServiceName).ToList();
                foreach (var item in settings)
                {
                    var jsonString = item.IsEncrypt
                        ? Encoding.ASCII.GetBytes(item.Settings.Decrypt(builder.Configuration.AesKey()))
                        : Encoding.ASCII.GetBytes(item.Settings);
                    builder.Configuration.
                        AddJsonStream(new MemoryStream(jsonString));
                }
                builder.Configuration.Build();

                builder.Services.TryAddSingleton(
                    m => new MixEndpointService(
                        builder.Configuration,
                        settings.First(m => m.SystemName == "endpoints")));
                builder.Services.TryAddSingleton(
                    m => new PortalConfigService(
                        builder.Configuration,
                        settings.First(m => m.SystemName == "portal")));
                builder.Services.TryAddSingleton(
                    m => new AuthConfigService(
                        builder.Configuration,
                        settings.First(m => m.SystemName == "authentication")));
                builder.Services.TryAddSingleton(m => new GlobalSettingsService(
                    builder.Configuration,
                    settings.First(m => m.SystemName == "global")));
                builder.Services.TryAddSingleton(m => new DatabaseService(
                   builder.Services.GetService<IHttpContextAccessor>(),
                   builder.Configuration,
                   settings.First(m => m.SystemName == "database")));

                builder.Services.TryAddSingleton(m => new SmtpConfigService(builder.Configuration, settings.First(m => m.SystemName == "smtp")));
                builder.Services.TryAddSingleton(m => new MixHeartConfigService(builder.Configuration, settings.First(m => m.SystemName == "mix_heart")));
                builder.Services.TryAddSingleton(m => new IPSecurityConfigService(builder.Configuration, settings.First(m => m.SystemName == "ip")));
                
                builder.Services.TryAddSingleton<MixPermissionService>();
                return builder;
            }
        }

        private static GlobalSettingContext GetSettingDbContext(IConfiguration configuration)
        {
            if (!Enum.TryParse<MixDatabaseProvider>(configuration["DatabaseProvider"], out var databaseProvider))
            {
                return new SqliteGlobalSettingContext(configuration);
            }

            return databaseProvider switch
            {
                MixDatabaseProvider.SQLITE => new SqliteGlobalSettingContext(configuration),
                MixDatabaseProvider.PostgreSQL => new PostgresGlobalSettingContext(configuration),
                MixDatabaseProvider.MySQL => new MySqlGlobalSettingContext(configuration),
                MixDatabaseProvider.SQLSERVER => new SqlServerGlobalSettingContext(configuration),
                _ => throw new NotImplementedException()

            };
        }
    }
}