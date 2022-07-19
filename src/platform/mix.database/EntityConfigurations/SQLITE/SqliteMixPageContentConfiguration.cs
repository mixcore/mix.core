namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixPageContentConfiguration : MixPageContentConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);
        }
    }
}
