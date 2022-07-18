namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDiscussionConfiguration : MixDiscussionConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);
        }
    }
}
