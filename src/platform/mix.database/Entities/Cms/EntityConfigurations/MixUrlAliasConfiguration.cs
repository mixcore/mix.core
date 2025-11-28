using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixUrlAliasConfiguration : TenantEntityBaseConfiguration<MixUrlAlias, int>
    {
        public MixUrlAliasConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixUrlAlias> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Alias)
                .HasColumnName("alias")
               .HasColumnType($"{Config.NString}{Config.SmallLength}");
            builder.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion(new EnumToStringConverter<MixUrlAliasType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}");
            builder.Property(e => e.SourceContentId)
              .HasColumnName("source_content_id")
              .HasColumnType(Config.Integer);
            builder.Property(e => e.SourceContentGuidId)
              .HasColumnName("source_content_guid_id")
              .HasColumnType(Config.Guid);

        }
    }
}
