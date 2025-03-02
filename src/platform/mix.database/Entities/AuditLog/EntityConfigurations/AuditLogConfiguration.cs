using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations;
using Newtonsoft.Json.Linq;
using Mix.Database.EntityConfigurations.Base;
using Microsoft.Extensions.DependencyModel.Resolution;
using Mix.Database.Services.MixGlobalSettings;
namespace Mix.Database.Entities.AuditLog.EntityConfigurations
{
    internal class AuditLogConfiguration : EntityBaseConfiguration<AuditLog, Guid>

    {
        public AuditLogConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Success)
                .HasColumnName("success")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.StatusCode)
                .HasColumnName("status_code")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ResponseTime)
               .HasColumnName("response_time")
               .HasColumnType(Config.Integer);

            builder.Property(e => e.Success)
                .HasColumnName("success")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.RequestIp)
                .HasColumnName("request_ip")
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Endpoint)
                .HasColumnName("endpoint")
                .HasColumnType($"{Config.NString}{Config.MaxLength}");

            builder.Property(e => e.QueryString)
                .HasColumnName("query_string")
                .HasColumnType($"{Config.NString}{Config.MaxLength}");

            builder.Property(e => e.Method)
                .HasColumnName("method")
                .HasColumnType($"{Config.NString}{Config.SmallLength}");

            builder.Property(e => e.Body)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnName("body")
             .HasColumnType(Config.Text);

            builder.Property(e => e.Response)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnName("response")
             .HasColumnType(Config.Text);

            builder.Property(e => e.Exception)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
             .IsRequired(false)
             .HasColumnName("exception")
             .HasColumnType(Config.Text);
        }
    }
}
