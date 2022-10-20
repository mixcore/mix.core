using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Database.Services;
using Mix.Services.Permission.Domain.Entities;

namespace Mix.Services.Permission.Domain.Entities.EntityConfigurations
{
    public class PermissionEndpointConfiguration : EntityBaseConfiguration<MixPermissionEndpoint, int>
    {
        public PermissionEndpointConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPermissionEndpoint> builder)
        {
            builder.ToTable(MixDatabaseNames.SYSTEM_PERMISSION_ENDPOINT);
            base.Configure(builder);
        }
    }
}
