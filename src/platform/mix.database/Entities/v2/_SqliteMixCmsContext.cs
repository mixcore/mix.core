using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE;
using Mix.Database.Extensions;

namespace Mix.Database.Entities.v2
{
    public class SqliteMixCmsContext : MixCmsContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly, 
                typeof(SqliteDatabaseConstants).Namespace);
        }
    }
}
