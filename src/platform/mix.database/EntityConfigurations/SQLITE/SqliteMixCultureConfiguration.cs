namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixCultureConfiguration : MixCultureConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);
        }
    }
}
