using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDataContentValueConfiguration : MixDataContentValueConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentValue> builder)
        {
            base.Configure(builder);
        }
    }
}
