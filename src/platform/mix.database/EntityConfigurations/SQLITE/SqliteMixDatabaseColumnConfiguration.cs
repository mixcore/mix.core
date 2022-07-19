namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDatabaseColumnConfiguration : MixDatabaseColumnConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);
        }
    }
}
