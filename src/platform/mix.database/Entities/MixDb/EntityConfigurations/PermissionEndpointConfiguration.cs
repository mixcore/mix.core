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
               .IsRequired(false);
            builder.Property(p => p.Description)
               .IsRequired(false);
            builder.Property(p => p.Path)
               .IsRequired(false);
            builder.Property(p => p.Method)
               .IsRequired(false);
        }
    }
}
