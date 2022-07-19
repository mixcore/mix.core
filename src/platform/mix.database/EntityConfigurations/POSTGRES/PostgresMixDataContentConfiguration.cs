namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDataContentConfiguration : MixDataContentConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
        }
    }
}
