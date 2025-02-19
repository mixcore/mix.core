using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixDbEventSubscriberConfiguration : EntityBaseConfiguration<MixDbEventSubscriber, int>
    {
        public MixDbEventSubscriberConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixDbEventSubscriber> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDbDatabaseNames.MixDbEventSubscriber);
            builder.Property(e => e.Callback)
                .HasColumnName("callback")
                .HasConversion(
                 v => v.ToString(Newtonsoft.Json.Formatting.None),
                 v => !string.IsNullOrEmpty(v) ? JObject.Parse(v) : new())
                .IsRequired(false)
                .HasColumnType(Config.Text);
            builder.Property(e => e.MixDbName)
               .HasColumnName("mixdb_name");
            builder.Property(e => e.Action)
               .HasColumnName("action");
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");
        }
    }
}
