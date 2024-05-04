using Mix.Database.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class PostgresQueueLogDbContext : QueueLogDbContext
    {
        public PostgresQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
