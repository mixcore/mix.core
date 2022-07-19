namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDataContentAssociationConfiguration : MixDataContentAssociationConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDataContentAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
