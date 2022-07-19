namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixPageConfiguration : MixPageConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPage> builder)
        {
            base.Configure(builder);
        }
    }
}
