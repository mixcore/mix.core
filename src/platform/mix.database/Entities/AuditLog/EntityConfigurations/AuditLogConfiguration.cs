using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.AuditLog.EntityConfigurations
{
    internal class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>

    {
        public AuditLogConfiguration()        {
        }

        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.Property(e => e.Exception)
             .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => JObject.Parse(v ?? "{}"))
             .IsRequired(false)
             .HasColumnType("ntext");
        }
    }
}
