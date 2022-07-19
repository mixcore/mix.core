namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixPostConfiguration : MixPostConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixPost> builder)
        {
            base.Configure(builder);
        }
    }
}
