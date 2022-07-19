namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDiscussionConfiguration : MixDiscussionConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);
        }
    }
}
