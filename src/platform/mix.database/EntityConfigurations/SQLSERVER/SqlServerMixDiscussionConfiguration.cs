using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDiscussionConfiguration : MixDiscussionConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);
        }
    }
}
