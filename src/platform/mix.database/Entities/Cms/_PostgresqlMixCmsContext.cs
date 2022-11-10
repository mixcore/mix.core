using Mix.Database.Services;

namespace Mix.Database.Entities.Cms
{
    public class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
