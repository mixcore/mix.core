using Microsoft.AspNetCore.Http;
using Mix.Database.EntityConfigurations.SQLITE;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext(IHttpContextAccessor httpContextAccessor, DatabaseService databaseService) : base(httpContextAccessor, databaseService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(SqliteDatabaseConstants).Namespace);
        }
    }
}
