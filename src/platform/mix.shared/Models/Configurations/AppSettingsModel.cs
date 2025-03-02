using Mix.Heart.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Models.Configurations
{
    public class AppSettingsModel
    {
        public AppSettingsModel() { }
        public MixDatabaseProvider DatabaseProvider { get; set; } = MixDatabaseProvider.SQLITE;
        public bool IsInit { get; set; }
        public bool IsHttps { get; set; }
        public string AesKey { get; set; }
        public string DefaultCulture { get; set; }
        public string HttpScheme { get; set; } = "https";
        public InitStep InitStatus { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
