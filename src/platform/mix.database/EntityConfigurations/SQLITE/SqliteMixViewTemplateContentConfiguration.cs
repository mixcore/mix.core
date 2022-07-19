namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixViewTemplateContentConfiguration : MixViewTemplateContentConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixTemplate> builder)
        {
            base.Configure(builder);
        }
    }
}
