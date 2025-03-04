﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Constants;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;


namespace Mix.Services.Graphql.Lib.Entities.EntityConfigurations
{
    public class MixMetadataConfiguration : EntityBaseConfiguration<MixMetadata, int>
    {
        public MixMetadataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMetadata> builder)
        {
            builder.ToTable(MixDbDatabaseNames.Metadata);
            base.Configure(builder);
        }
    }
}
