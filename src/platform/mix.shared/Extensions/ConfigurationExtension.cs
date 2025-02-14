using Microsoft.Extensions.Configuration;
using Mix.Heart.Enums;
using Mix.Shared.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Extensions
{
    public static class ConfigurationExtension
    {
        public static T GetGlobalConfiguration<T>(
            this IConfiguration configuration,
            string key)
        {
            return configuration.GetSection(MixAppSettingsSection.GlobalSettings).GetValue<T>(key);
        }

        public static MixDatabaseProvider DatabaseProvider(this IConfiguration configuration)
        {
            if (Enum.TryParse<MixDatabaseProvider>(configuration["DatabaseProvider"], out var provider))
            {
                return provider;
            }
            return MixDatabaseProvider.SQLITE;
        }

        public static bool IsInit(this IConfiguration configuration)
        {
            return configuration.GetValue<bool>(nameof(AppSettingsModel.IsInit));
        }

        public static InitStep InitStep(this IConfiguration configuration)
        {
            return configuration.GetValue<InitStep>(nameof(AppSettingsModel.InitStatus));
        }
        
        public static string? AesKey(this IConfiguration configuration)
        {
            return configuration["AesKey"];
        }
        
        public static string? SettingsConnection(this IConfiguration configuration)
        {
            return configuration.GetValue<string>(MixConstants.CONST_SETTINGS_CONNECTION) ?? "Data Source=wwwroot\\mixcontent\\settings.sqlite";
        }
    }
}
