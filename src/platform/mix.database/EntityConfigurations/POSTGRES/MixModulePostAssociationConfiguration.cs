using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.POSTGRES.Base;

namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixModulePostAssociationConfiguration : PostgresEntityBaseConfiguration<MixModulePostAssociation, int>
    {
        public override void Configure(EntityTypeBuilder<MixModulePostAssociation> builder)
        {
            base.Configure(builder);
        }
    }
}
