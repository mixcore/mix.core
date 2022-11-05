using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDiscussionConfiguration : MixDiscussionConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);
        }
    }
}
