using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.POSTGRES.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES
{
    public class MixDataContentConfiguration : PostgresMultilanguageSEOContentBaseConfiguration<MixDataContent, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
        }
    }
}
