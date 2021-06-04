using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;

namespace Mix.Database.EntityConfigurations.v2.SQLSERVER.Base
{
    public abstract class MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey> : MultilanguageContentBaseConfiguration<T, TPrimaryKey>
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType(DataTypes.DateTime);

            builder.Property(e => e.Image)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MediumLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{DataTypes.NString}{DatabaseConfiguration.MaxLength}")
                .HasCharSet(DatabaseConfiguration.CharSet)
                .UseCollation(DatabaseConfiguration.DatabaseCollation);

        }

    }
}
