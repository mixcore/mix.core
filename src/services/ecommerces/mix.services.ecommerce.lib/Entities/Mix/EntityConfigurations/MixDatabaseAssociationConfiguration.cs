﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;

namespace Mix.Services.Ecommerce.Lib.Entities.Mix.EntityConfigurations
{
    public class MixDatabaseAssociationConfiguration : EntityBaseConfiguration<MixDatabaseAssociation, Guid>

    {
        public MixDatabaseAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ParentDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.ChildDatabaseName)
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}