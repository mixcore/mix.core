namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDatabaseRelationshipConfiguration : MixDatabaseRelationshipConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseRelationship> builder)
        {
            base.Configure(builder);
        }
    }
}
