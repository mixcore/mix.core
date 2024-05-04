using Mix.Database.Services;

namespace Mix.Database.Entities.QueueLog
{
    public class MySqlQueueLogDbContext : QueueLogDbContext
    {
        public MySqlQueueLogDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
