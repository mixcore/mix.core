namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDatabaseContextConfiguration : MixDatabaseContextConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);
        }
    }
}
