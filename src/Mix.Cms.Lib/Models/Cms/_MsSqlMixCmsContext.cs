using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Extensions;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MsSqlMixCmsContext : MixCmsContext
    {
        public MsSqlMixCmsContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly,
                "Mix.Cms.Lib.Models.EntityConfigurations.MSSQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}