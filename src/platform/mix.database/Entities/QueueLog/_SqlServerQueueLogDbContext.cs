using Mix.Database.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class SqlServerQueueLogDbContext : QueueLogDbContext
    {
        public SqlServerQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
