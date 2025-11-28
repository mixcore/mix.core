using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;

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
            builder.ToTable("mix_audit_log");
            
            builder.Property(e => e.Success)
                .HasColumnName("success")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.StatusCode)
                .HasColumnName("status_code")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.ResponseTime)
               .HasColumnName("response_time")
               .HasColumnType(Config.Integer);

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

            // HIPAA/GDPR Compliance Enhancement Fields
            builder.Property(e => e.CorrelationId)
                .HasColumnName("correlation_id")
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id")
                .HasColumnType(Config.Integer);

            builder.Property(e => e.PhiAccessFlag)
                .HasColumnName("phi_access_flag")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.UserAgent)
                .HasColumnName("user_agent")
                .HasColumnType($"{Config.String}{Config.MaxLength}");

            builder.Property(e => e.SessionId)
                .HasColumnName("session_id")
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            // Indexes for compliance queries
            builder.HasIndex(e => new { e.TenantId, e.PhiAccessFlag })
                .HasDatabaseName("IX_AuditLog_Tenant_PHI");

            builder.HasIndex(e => e.CorrelationId)
                .HasDatabaseName("IX_AuditLog_CorrelationId");
        }
    }
}
