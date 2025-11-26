using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
