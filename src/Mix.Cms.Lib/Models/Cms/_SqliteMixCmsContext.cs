using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;

namespace Mix.Cms.Lib.Models.Cms
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
                "Mix.Cms.Lib.Models.EntityConfigurations.SQLITE");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}