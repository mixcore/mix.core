namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixMediaConfiguration : MixMediaConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            base.Configure(builder);
        }
    }
}
