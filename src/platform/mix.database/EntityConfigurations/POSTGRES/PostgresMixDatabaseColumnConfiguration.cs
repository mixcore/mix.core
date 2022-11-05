using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDatabaseColumnConfiguration : MixDatabaseColumnConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);
        }
    }
}
