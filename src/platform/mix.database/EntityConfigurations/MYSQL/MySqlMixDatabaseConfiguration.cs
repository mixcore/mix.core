using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDatabaseConfiguration : MixDatabaseConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);
        }
    }
}
