namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDatabaseColumnConfiguration : MixDatabaseColumnConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseColumn> builder)
        {
            base.Configure(builder);
        }
    }
}
