using Mix.Database.Base.Cms;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixUrlAliasConfiguration : TenantEntityBaseConfiguration<MixUrlAlias, int>
    {
        public MixUrlAliasConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
