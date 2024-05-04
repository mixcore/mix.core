using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb
{
    public class SqliteMixDbDbContext : MixDbDbContext
    {
        public SqliteMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
