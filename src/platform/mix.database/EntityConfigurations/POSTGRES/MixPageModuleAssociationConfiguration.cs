using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixPageModuleAssociationConfiguration : PostgresAssociationBaseConfiguration<MixPageModuleAssociation, int>
    {
        public override void Configure(EntityTypeBuilder<MixPageModuleAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
