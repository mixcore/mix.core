namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixModuleContentConfiguration : MixModuleContentConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);
        }
    }
}
