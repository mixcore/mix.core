namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixUrlAliasConfiguration : MixUrlAliasConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
        }
    }
}
