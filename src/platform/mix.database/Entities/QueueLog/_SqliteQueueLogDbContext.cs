using Mix.Database.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class SqlITEQueueLogDbContext : QueueLogDbContext
    {
        public SqlITEQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
