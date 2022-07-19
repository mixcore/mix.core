namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDatabaseContextConfiguration : MixDatabaseContextConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);
        }
    }
}
