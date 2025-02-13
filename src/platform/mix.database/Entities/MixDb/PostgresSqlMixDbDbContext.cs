using Mix.Database.Services.MixGlobalSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.MixDb
{
    public class PostgresSqlMixDbDbContext : MixDbDbContext
    {
        public PostgresSqlMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
