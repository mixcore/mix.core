using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
