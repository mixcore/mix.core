namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixModuleConfiguration : MixModuleConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);
        }
    }
}
