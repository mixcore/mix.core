using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.POSTGRES.Base
{
     public abstract class MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey> : MultilanguageContentBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Title)
               .IsRequired()
               .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
               .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
               .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Content)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.Text}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Layout)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Template)
              .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType(PostgresSqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.Image)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{PostgresSqlDatabaseConstants.DataTypes.NString}{PostgresSqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(PostgresSqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(PostgresSqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

        }

    }
}
