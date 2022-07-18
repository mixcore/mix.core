using Mix.Database.EntityConfigurations.MYSQL.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDataConfiguration : MixDataConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);
        }
    }
}
