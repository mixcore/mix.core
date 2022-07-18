namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixModuleConfiguration : MixModuleConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);
        }
    }
}
