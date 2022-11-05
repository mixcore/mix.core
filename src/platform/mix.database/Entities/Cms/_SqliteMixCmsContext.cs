using Microsoft.AspNetCore.Http;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(DatabaseService databaseService) : base(databaseService)
        {
        }
    }
}
