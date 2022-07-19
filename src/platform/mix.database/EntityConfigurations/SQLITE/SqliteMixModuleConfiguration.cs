namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixModuleConfiguration : MixModuleConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);
        }
    }
}
