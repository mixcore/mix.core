using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixCultureConfiguration : MixCultureConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);
        }
    }
}
