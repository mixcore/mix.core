using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPageConfiguration : TenantEntityBaseConfiguration<MixPage, int>
        
    {
        public MixPageConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
