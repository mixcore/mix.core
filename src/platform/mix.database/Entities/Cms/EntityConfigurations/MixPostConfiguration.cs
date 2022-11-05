using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPostConfiguration : EntityBaseConfiguration<MixPost, int>
        
    {
        public MixPostConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
