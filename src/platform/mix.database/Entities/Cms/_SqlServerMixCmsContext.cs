using Mix.Database.Services;

namespace Mix.Database.Entities.Cms
{
    public class SqlServerMixCmsContext : MixCmsContext
    {
        public SqlServerMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
