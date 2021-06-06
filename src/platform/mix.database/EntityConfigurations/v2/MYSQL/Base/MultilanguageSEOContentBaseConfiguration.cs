using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Base;
using System;

namespace Mix.Database.EntityConfigurations.v2.MYSQL.Base
{
    public abstract class MultilanguageSEOContentBaseConfiguration<T, TPrimaryKey> 
        : MultilanguageContentBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : MultilanguageSEOContentBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Layout)
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Template)
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MixDatabaseName)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.PublishedDateTime)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.DateTime);

            builder.Property(e => e.Image)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Source)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoDescription)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoKeywords)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.SeoName)
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MaxLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

        }

    }
}
