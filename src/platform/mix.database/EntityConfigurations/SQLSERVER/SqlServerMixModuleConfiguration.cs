using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixModuleConfiguration : MixModuleConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);
        }
    }
}
