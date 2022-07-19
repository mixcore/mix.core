namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixCultureConfiguration : MixCultureConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixCulture> builder)
        {
            base.Configure(builder);
        }
    }
}
