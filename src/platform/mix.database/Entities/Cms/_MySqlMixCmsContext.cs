using Microsoft.AspNetCore.Http;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.Services;

namespace Mix.Database.Entities
{
    public class MySqlMixCmsContext : MixCmsContext
    {
        public MySqlMixCmsContext(IHttpContextAccessor httpContextAccessor,DatabaseService databaseService) : base(httpContextAccessor, databaseService)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                this.GetType().Assembly,
                m => m.Namespace == typeof(MySqlDatabaseConstants).Namespace);
        }
    }
}
