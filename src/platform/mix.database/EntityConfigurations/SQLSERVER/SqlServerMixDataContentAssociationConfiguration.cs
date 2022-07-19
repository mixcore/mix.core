namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class SqlServerMixDataContentAssociationConfiguration : MixDataContentAssociationConfiguration<SqlServerDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
