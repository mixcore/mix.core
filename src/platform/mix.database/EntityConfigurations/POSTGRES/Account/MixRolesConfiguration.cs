﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using Mix.Database.EntityConfigurations.Base.Account;

namespace Mix.Database.EntityConfigurations.POSTGRES.Account
{
    internal class MixRolesConfiguration : MixRolesConfiguration<PostgresDatabaseConstants>
    {
        public override void Configure(EntityTypeBuilder<MixRole> builder)
        {
            base.Configure(builder);
        }
    }
}