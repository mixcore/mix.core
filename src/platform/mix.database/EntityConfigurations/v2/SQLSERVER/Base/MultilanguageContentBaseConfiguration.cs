using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class MultilanguageContentBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where T : MultilanguageContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.SmallLength}")
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

            builder.Property(e => e.Content)
                .IsRequired()
                .HasColumnType(DataTypes.Text)
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);
        }

    }
}
