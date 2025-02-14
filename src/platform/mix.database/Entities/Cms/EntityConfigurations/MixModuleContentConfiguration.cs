using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixModuleContentConfiguration : MultilingualSEOContentBaseConfiguration<MixModuleContent, int>

    {
        public MixModuleContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixModuleContent> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.SimpleDataColumns)
              .HasColumnName("simple_data_columns")
              .HasColumnType(Config.Text);

            builder.Property(e => e.PageSize)
              .HasColumnName("page_size");

            builder.Property(e => e.MixDbId)
              .HasColumnName("mix_db_id");

            builder.Property(e => e.MixDatabaseName)
              .HasColumnName("mix_database_name")
              .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.SystemName)
                .HasColumnName("system_name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.ClassName)
                .HasColumnName("class_name")
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<MixModuleType>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.HasOne(e => e.MixModule)
                .WithMany(e => e.MixModuleContents)
                .HasForeignKey(m => m.ParentId);
        }
    }
}
