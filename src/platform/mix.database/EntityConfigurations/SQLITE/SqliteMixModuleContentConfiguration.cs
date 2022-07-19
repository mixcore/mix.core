namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixModuleContentConfiguration : MixModuleContentConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);
        }
    }
}
