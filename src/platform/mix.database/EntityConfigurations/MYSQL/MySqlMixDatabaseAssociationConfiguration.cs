using Mix.Database.Base;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MySqlMixDatabaseAssociationConfiguration : MixDatabaseAssociationConfiguration<MySqlDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
