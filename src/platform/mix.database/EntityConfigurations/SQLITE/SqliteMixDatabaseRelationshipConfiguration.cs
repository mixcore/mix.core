namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDatabaseRelationshipConfiguration : MixDatabaseRelationshipConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseRelationship> builder)
        {
            base.Configure(builder);
        }
    }
}
