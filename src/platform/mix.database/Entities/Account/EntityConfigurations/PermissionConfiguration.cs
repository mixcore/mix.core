using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    public class PermissionConfiguration : EntityBaseConfiguration<Permission, int>

    {
        public PermissionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(MixDatabaseNames.SYSTEM_PERMISSION);
            base.Configure(builder);
        }
    }
}
