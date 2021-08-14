using Mix.Heart.Enums;
using Mix.Shared.Abstracts;
using Mix.Shared.Constants;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Shared.Services
{
    public class GlobalConfigService : JsonConfigurationServiceBase
    {
        public List<string> Cultures { get; set; }

        public GlobalConfigService() : base(MixAppConfigFilePaths.Global)
        {
        }

        public MixDatabaseProvider DatabaseProvider
        {
            get => GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
        }

        public string GetConnectionString(string name)
        {
            var result = AppSettings["ConnectionStrings"][name];
            return result != null ? result.Value<string>() : string.Empty;
        }

        public void SetConnectionString(string name, string value)
        {
            AppSettings["ConnectionStrings"][name] = value;
        }
    }
}
