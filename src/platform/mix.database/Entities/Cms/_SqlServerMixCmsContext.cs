using Microsoft.AspNetCore.Http;
using Mix.Database.EntityConfigurations.SQLSERVER;
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
