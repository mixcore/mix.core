using Mix.Database.EntityConfigurations.SQLITE.Base;

namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class MixPageConfiguration : SqliteTenantEntityBaseConfiguration<MixPage, int>
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
