﻿using Mix.Shared.Models.Configurations;

namespace Mix.Common.Models
{
    public class GlobalSettings

    {
        public string Domain { get; set; }

        public string DefaultCulture { get; set; }

        public string LangIcon { get; set; }

        public PortalConfigurationModel PortalThemeSettings { get; set; }

        public string ApiEncryptKey { get; set; }

        public Dictionary<string, string> RSAKeys { get; set; }

        public bool IsEncryptApi { get; set; }

        public string[] PageTypes { get; set; }

        public string[] ModuleTypes { get; set; }

        public string[] MixDatabaseTypes { get; set; }

        public string[] DataTypes { get; set; }

        public string[] Statuses { get; set; }

        public JObject ExternalLoginProviders { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }

        public DateTime? ExpiredAt { get; set; } = DateTime.UtcNow.AddMinutes(20);
    }
}
