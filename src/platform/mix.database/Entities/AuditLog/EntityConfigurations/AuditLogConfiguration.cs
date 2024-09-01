using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;
using Mix.Database.EntityConfigurations.Base;
using Microsoft.Extensions.DependencyModel.Resolution;
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
