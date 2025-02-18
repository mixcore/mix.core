using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class PermissionEndpointConfiguration : EntityBaseConfiguration<MixPermissionEndpoint, int>
    {
        public PermissionEndpointConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPermissionEndpoint> builder)
        {
            base.Configure(builder);
            builder.ToTable(MixDatabaseNames.SYSTEM_PERMISSION_ENDPOINT);
            builder.Property(p => p.Title)
                .HasColumnName("title")
            builder.Property(p => p.Description)
               .HasColumnName("description");
            builder.Property(p => p.Path)
               .HasColumnName("path");
            builder.Property(p => p.TenantId)
               .HasColumnName("tenant_id");
            builder.Property(p => p.Method)
               .HasColumnName("method");
            builder.Property(p => p.MixPermissionId)
               .HasColumnName("mix_permission_id");
        }
    }
}
