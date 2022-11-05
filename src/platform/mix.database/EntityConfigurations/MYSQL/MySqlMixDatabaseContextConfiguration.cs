using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDatabaseContextConfiguration : MixDatabaseContextConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseContext> builder)
        {
            base.Configure(builder);
        }
    }
}
