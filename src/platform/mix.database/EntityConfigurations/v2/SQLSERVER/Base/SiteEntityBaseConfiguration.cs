using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class SiteEntityBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where T : SiteEntityBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Image)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.DisplayName)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SystemName)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

        }

    }
}
