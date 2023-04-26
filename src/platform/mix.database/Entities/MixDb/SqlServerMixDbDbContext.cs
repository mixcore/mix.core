using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb
{
    public class SqlServerMixDbDbContext : MixDbDbContext
    {
        public SqlServerMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
