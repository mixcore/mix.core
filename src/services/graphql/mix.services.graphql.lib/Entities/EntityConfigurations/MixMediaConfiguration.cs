﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Mixdb;
using Mix.Mixdb.Entities;

namespace Mix.Services.Graphql.Lib.Entities.EntityConfigurations
{
    public class MixMediaConfiguration : EntityBaseConfiguration<MixMedia, int>
    {
        public MixMediaConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMedia> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNameMedia);
            base.Configure(builder);
        }
    }
}
