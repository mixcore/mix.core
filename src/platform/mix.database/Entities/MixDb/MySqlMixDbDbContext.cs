using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb
{
    public class MySqlMixDbDbContext : MixDbDbContext
    {
        public MySqlMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
