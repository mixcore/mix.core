using Mix.Database.Services;

namespace Mix.Database.Entities.Cms
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
