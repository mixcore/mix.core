namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDatabaseRelationshipConfiguration : MixDatabaseRelationshipConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseRelationship> builder)
        {
            base.Configure(builder);
        }
    }
}
