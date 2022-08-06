namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDatabaseAssociationConfiguration : MixDatabaseAssociationConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
