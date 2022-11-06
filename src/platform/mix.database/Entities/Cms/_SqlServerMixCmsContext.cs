using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class SqlServerMixCmsContext : MixCmsContext
    {
        public SqlServerMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
