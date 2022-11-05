using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDatabaseConfiguration : MixDatabaseConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);
        }
    }
}
