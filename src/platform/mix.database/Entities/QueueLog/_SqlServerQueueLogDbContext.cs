using Mix.Database.Entities.QueueLog.EntityConfigurations;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class SqlServerQueueLogDbContext : QueueLogDbContext
    {
        public SqlServerQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
