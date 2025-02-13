using Mix.Database.Base.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixApplicationConfiguration : TenantEntityBaseConfiguration<MixApplication, int>

    {
        public MixApplicationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixApplication> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.TemplateId)
                .HasColumnName("template_id")
               .HasColumnType(Config.Integer);
            builder.Property(e => e.MixDbId)
                .HasColumnName("mix_db_id")
               .HasColumnType(Config.Integer);
            builder.Property(e => e.Image)
                .HasColumnName("image")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.DisplayName)
                .HasColumnName("display_name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.AppSettings)
                .HasColumnName("app_settings")
               .HasColumnType(Config.Text)
               .HasConversion(
                   v => v.ToString(Newtonsoft.Json.Formatting.None),
                   v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
               .HasCharSet(Config.CharSet);
            builder.Property(e => e.BaseHref)
                .HasColumnName("base_href")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.DeployUrl)
                .HasColumnName("deploy_url")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.Domain)
                .HasColumnName("domain")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.BaseApiUrl)
                .HasColumnName("base_api_url")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
            builder.Property(e => e.MixDatabaseName)
                .HasColumnName("mix_database_name")
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
