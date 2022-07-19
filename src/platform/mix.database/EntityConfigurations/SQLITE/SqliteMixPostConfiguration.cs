namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixPostConfiguration : MixPostConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
