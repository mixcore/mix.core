using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class PostgresMixDatabaseAssociationConfiguration : MixDatabaseAssociationConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
