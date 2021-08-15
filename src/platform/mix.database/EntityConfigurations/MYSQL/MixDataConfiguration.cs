using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.MYSQL.Base;
using System;

namespace Mix.Database.EntityConfigurations.MYSQL
{
    public class MixDataConfiguration : MySqlEntityBaseConfiguration<MixData, Guid>
    {
        public override void Configure(EntityTypeBuilder<MixData> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnType($"{Config.NString}{Config.MediumLength}")
              .HasCharSet(Config.CharSet)
              .UseCollation(Config.DatabaseCollation);
        }
    }
}
