using Microsoft.Extensions.Configuration;
using Mix.Database.Services;
using Mix.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Settings
{
    public class MySqlGlobalSettingContext : GlobalSettingContext
    {
        public MySqlGlobalSettingContext(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
