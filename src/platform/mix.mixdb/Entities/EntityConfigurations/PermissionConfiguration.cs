﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Constant.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Service.Services;

namespace Mix.Mixdb.Entities.EntityConfigurations
{
    public class PermissionConfiguration : EntityBaseConfiguration<MixPermission, int>
    {
        public PermissionConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixPermission> builder)
        {
            try
            {
                base.Configure(builder);
                builder.ToTable(MixDatabaseNames.SYSTEM_PERMISSION);
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
            }
        }
    }
}