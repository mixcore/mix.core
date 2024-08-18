using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;
using Mix.Database.EntityConfigurations.Base;
using Microsoft.Extensions.DependencyModel.Resolution;
namespace Mix.Database.Entities.AuditLog.EntityConfigurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>

    {
        public IDatabaseConstants Config;
        public AuditLogConfiguration()
        {
            Config = new SqliteDatabaseConstants();
        }

        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(e => e.IsDeleted)
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.CreatedDateTime)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ModifiedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Priority)
                .HasColumnType(Config.Integer);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);

            builder.Property(e => e.Body)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);

            builder.Property(e => e.Response)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);

            builder.Property(e => e.Exception)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnType(Config.Text);
        }
    }
}
