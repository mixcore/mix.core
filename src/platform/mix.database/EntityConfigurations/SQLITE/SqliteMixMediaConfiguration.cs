namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixMediaConfiguration : MixMediaConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            base.Configure(builder);
        }
    }
}
