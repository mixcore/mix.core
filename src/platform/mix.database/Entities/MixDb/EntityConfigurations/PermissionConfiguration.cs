using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

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
        }
    }
}
