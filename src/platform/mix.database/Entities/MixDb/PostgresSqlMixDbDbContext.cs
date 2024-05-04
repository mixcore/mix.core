using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb
{
    public class PostgresSqlMixDbDbContext : MixDbDbContext
    {
        public PostgresSqlMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
