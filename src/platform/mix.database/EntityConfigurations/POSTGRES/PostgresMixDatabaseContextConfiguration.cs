namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDatabaseContextConfiguration : MixDatabaseContextConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);
        }
    }
}
