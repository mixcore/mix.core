using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDomainConfiguration : MixDomainConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDomain> builder)
        {
            base.Configure(builder);
        }
    }
}
