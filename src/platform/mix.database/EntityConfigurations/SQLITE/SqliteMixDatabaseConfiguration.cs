namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDatabaseConfiguration : MixDatabaseConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);
        }
    }
}
