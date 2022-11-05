using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDomainConfiguration : MixDomainConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
