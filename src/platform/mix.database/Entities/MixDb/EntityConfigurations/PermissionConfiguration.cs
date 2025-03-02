using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class PermissionConfiguration : EntityBaseConfiguration<MixPermission, int>
    {
        public PermissionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPermission> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDatabaseNames.SYSTEM_PERMISSION);
            builder.Property(e => e.DisplayName)
            .HasColumnName("display_name");
            builder.Property(e => e.Group)
               .HasColumnName("group");
            builder.Property(e => e.Key)
               .HasColumnName("key");
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");

        }
    }
}
