using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixDataContentConfiguration : SqliteMultilanguageSEOContentBaseConfiguration<MixDataContent, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
        }
    }
}
