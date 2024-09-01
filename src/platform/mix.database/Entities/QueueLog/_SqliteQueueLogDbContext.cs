using Mix.Database.Entities.QueueLog.EntityConfigurations;
using Mix.Database.Services;
using Mix.Heart.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class SqlITEQueueLogDbContext : QueueLogDbContext
    {
        public SqlITEQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
