using Microsoft.AspNetCore.Http;
using Mix.Database.EntityConfigurations.POSTGRES;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class PostgresqlMixCmsContext : MixCmsContext
    {
        public PostgresqlMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
