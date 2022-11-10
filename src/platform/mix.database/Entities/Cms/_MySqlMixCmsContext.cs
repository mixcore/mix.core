using Mix.Database.Services;

namespace Mix.Database.Entities.Cms
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
