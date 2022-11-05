using Microsoft.AspNetCore.Http;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
