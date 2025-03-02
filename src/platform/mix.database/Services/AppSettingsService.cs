using Microsoft.Extensions.Configuration;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Services
{
    public class AppSettingsService : AppSettingServiceBase<AppSettingsModel>
    {
        public AppSettingsService(
            IConfiguration configuration)
            : base(configuration, string.Empty, "appsettings", false)
        {
        }

        public override void SetConfig<TValue>(string name, TValue value, bool isSave = false)
        {
            base.SetConfig(name, value, isSave);
            _configuration[name] = value.ToString();
        }
    }

    public class ConnectionStrings
    {
        public string SettingsConnectionString { get; set; }
    }
}
