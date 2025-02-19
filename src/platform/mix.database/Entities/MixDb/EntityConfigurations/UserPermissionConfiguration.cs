using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class UserPermissionConfiguration : EntityBaseConfiguration<MixUserPermission, int>
    {
        public UserPermissionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixUserPermission> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDatabaseNames.SYSTEM_USER_PERMISSION);
            builder.Property(p => p.UserId)
                .HasColumnName("user_id");
            builder.Property(p => p.TenantId)
                .HasColumnName("tenant_id");
            builder.Property(p => p.PermissionId)
                .HasColumnName("permission_id");
            builder.Property(p => p.Description)
                .HasColumnName("description");
        }
    }
}
