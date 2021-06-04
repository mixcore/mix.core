using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.SQLITE.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.SQLITE
{
    public class MixViewTemplateContentConfiguration : EntityBaseConfiguration<MixViewTemplate, int>
    {
        public override void Configure(EntityTypeBuilder<MixViewTemplate> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.FileFolder)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.FileName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.FolderType)
              .HasConversion(new EnumToStringConverter<MixTemplateFolderType>())
              .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Scripts)
                .IsRequired()
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Styles)
                .IsRequired()
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.SpaContent)
                .IsRequired()
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MobileContent)
                .IsRequired()
                .HasColumnType(SqliteDatabaseConstants.DataTypes.Text)
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.MixThemeName)
                .IsRequired()
                .HasColumnType($"{SqliteDatabaseConstants.DataTypes.NString}{SqliteDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(SqliteDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(SqliteDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
