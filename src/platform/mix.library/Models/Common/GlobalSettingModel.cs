using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Mix.Lib.Models.Common
{
    public class GlobalSettingModel

    {
        public string Domain { get; set; }

        public string Lang { get; set; }

        public string LangIcon { get; set; }

        public JObject PortalThemeSettings { get; set; }

        public string ApiEncryptKey { get; set; }

        public Dictionary<string, string> RSAKeys { get; set; }

        public bool IsEncryptApi { get; set; }

        public List<string> Cultures { get; set; }

        public List<object> PageTypes { get; set; }

        public List<object> ModuleTypes { get; set; }

        public List<object> MixDatabaseTypes { get; set; }

        public List<object> DataTypes { get; set; }

        public List<object> Statuses { get; set; }

        public JObject ExternalLoginProviders { get; set; }

        public DateTime? LastUpdateConfiguration { get; set; }
    }
}
