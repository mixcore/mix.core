﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Cms;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixLanguageConfiguration : SqlServerSiteEntityUniqueNameBaseConfiguration<MixLanguage, int>
    {
        public override void Configure(EntityTypeBuilder<MixLanguage> builder)
        {
            base.Configure(builder);
        }
    }
}