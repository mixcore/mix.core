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
               .IsRequired(false);
            builder.Property(p => p.Description)
               .IsRequired(false)
               .HasColumnName("description");
            builder.Property(p => p.Path)
               .IsRequired(false)
               .HasColumnName("path");
            builder.Property(p => p.TenantId)
               .IsRequired(false)
               .HasColumnName("tenant_id");
            builder.Property(p => p.Method)
               .IsRequired(false)
               .HasColumnName("method");
            builder.Property(p => p.MixPermissionId)
               .IsRequired(false)
               .HasColumnName("mix_permission_id");
        }
    }
}
