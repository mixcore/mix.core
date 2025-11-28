using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb
{
    public class SqlServerMixDbDbContext : MixDbDbContext
    {
        public SqlServerMixDbDbContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
