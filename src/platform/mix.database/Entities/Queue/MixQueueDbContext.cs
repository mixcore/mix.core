using Mix.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Database.Entities.Queue
{
    public sealed class MixQueueDbContext : BaseDbContext
    {
        #region Contructors

        public MixQueueDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_CMS_CONNECTION)
        {
        }

        public MixQueueDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
        {
        }

        #endregion


    }
}
