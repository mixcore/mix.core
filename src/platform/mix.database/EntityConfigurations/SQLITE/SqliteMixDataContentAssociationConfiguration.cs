namespace Mix.Database.EntityConfigurations.SQLITE
{
    public class SqliteMixDataContentAssociationConfiguration : MixDataContentAssociationConfiguration<SqliteDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
