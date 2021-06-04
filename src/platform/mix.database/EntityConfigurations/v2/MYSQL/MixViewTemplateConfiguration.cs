using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Cms.v2;
using Mix.Database.EntityConfigurations.v2.MYSQL.Base;
using Mix.Shared.Enums;

namespace Mix.Database.EntityConfigurations.v2.MYSQL
{
    public class MixViewTemplateContentConfiguration : EntityBaseConfiguration<MixViewTemplate, int>
    {
        public override void Configure(EntityTypeBuilder<MixViewTemplate> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Extension)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.FileFolder)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.MediumLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.FileName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.FolderType)
              .HasConversion(new EnumToStringConverter<MixTemplateFolderType>())
              .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
              .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
              .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Scripts)
                .IsRequired()
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.Styles)
                .IsRequired()
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
            
            builder.Property(e => e.SpaContent)
                .IsRequired()
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

            builder.Property(e => e.MobileContent)
                .IsRequired()
                .HasColumnType(MySqlDatabaseConstants.DataTypes.Text)
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);

             builder.Property(e => e.MixThemeName)
                .IsRequired()
                .HasColumnType($"{MySqlDatabaseConstants.DataTypes.NString}{MySqlDatabaseConstants.DatabaseConfiguration.SmallLength}")
                .HasCharSet(MySqlDatabaseConstants.DatabaseConfiguration.CharSet)
                .UseCollation(MySqlDatabaseConstants.DatabaseConfiguration.DatabaseCollation);
        }
    }
}
