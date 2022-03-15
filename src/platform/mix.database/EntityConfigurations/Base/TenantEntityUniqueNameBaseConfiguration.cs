using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class TenantEntityUniqueNameBaseConfiguration<T, TPrimaryKey, TConfig> : TenantEntityBaseConfiguration<T, TPrimaryKey, TConfig>
        where TPrimaryKey : IComparable
        where T : TenantEntityUniqueNameBase<TPrimaryKey>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
               .IsRequired()
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

        }

    }
}
