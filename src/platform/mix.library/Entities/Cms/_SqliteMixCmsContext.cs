using Microsoft.EntityFrameworkCore;
using Mix.Heart.Extensions;

namespace Mix.Lib.Entities.Cms
{
    public partial class SqliteMixCmsContext : MixCmsContext
    {
        public SqliteMixCmsContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Lib.Models.EntityConfigurations.SQLITE");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}