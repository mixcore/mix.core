using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixDataContentConfiguration : MultilanguageSEOContentBaseConfiguration<MixDataContent, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixDataContent> builder)
        {
            base.Configure(builder);
        }
    }
}
