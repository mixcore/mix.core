using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Base.Cms;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDatabaseConfiguration : TenantEntityBaseConfiguration<MixDatabase, int>

    {
        public MixDatabaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SystemName)
               .IsRequired()
               .HasColumnName("system_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasColumnName("type")
               .HasConversion(new EnumToStringConverter<MixDatabaseType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.SelfManaged)
               .HasColumnName("self_managed");

            builder.Property(e => e.MixDatabaseContextId)
               .IsRequired(false)
               .HasColumnName("mix_database_context_id");

            builder.Property(e => e.ReadPermissions)
            .IsRequired(false)
            .HasColumnName("read_permissions")
            .HasColumnType($"{Config.NString}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
            .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.CreatePermissions)
               .IsRequired(false)
               .HasColumnName("create_permissions")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.UpdatePermissions)
               .IsRequired(false)
               .HasColumnName("update_permissions")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.DeletePermissions)
               .IsRequired(false)
               .HasColumnName("delete_permissions")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());
        }
    }
}
