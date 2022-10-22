using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.Services;
using Mix.Services.Permission.Domain.Entities;

namespace Mix.Services.Permission.Domain.Entities.EntityConfigurations
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
            builder.Property(p => p.Title)
                .IsRequired(false);
            builder.Property(p => p.Type)
                .IsRequired(false);
            builder.Property(p => p.Icon)
                .IsRequired(false);
        }
    }
}
