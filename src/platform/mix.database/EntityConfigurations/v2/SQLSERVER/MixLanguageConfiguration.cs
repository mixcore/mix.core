using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER
{
    public class MixLanguageConfiguration : EntityBaseConfiguration<MixLanguage, int>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
        }
    }
}
