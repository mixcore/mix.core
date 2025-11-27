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
    public class SqlServerGlobalSettingContext : GlobalSettingContext
    {
        public SqlServerGlobalSettingContext(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
