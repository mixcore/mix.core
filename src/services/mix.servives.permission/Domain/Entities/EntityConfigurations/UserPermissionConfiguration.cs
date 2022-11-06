using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Services.Permission.Domain.Entities.EntityConfigurations
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
