
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixPageContentConfiguration : MultilingualSEOContentBaseConfiguration<MixPageContent, int>
    {
        public MixPageContentConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixPageContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.PageSize)
              .HasColumnName("page_size")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.MixDatabaseName)
              .HasColumnName("mix_database_name")
              .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.MixDbId)
              .HasColumnName("mix_db_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.ClassName)
                .HasColumnName("class_name")
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.Type)
                .IsRequired()
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<MixPageType>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.HasOne(e => e.MixPage)
                .WithMany(e => e.MixPageContents)
                .HasForeignKey(m => m.ParentId);
        }
    }
}
