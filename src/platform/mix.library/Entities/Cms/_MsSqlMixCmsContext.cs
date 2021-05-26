using Microsoft.EntityFrameworkCore;
using Mix.Heart.Extensions;

namespace Mix.Lib.Entities.Cms
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
                "Mix.Lib.Models.EntityConfigurations.MSSQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}