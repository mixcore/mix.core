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
            builder.ToTable(MixDatabaseNames.SYSTEM_USER_PERMISSION);
            base.Configure(builder);
            builder.Property(p => p.Description)
               .IsRequired(false);
        }
    }
}
