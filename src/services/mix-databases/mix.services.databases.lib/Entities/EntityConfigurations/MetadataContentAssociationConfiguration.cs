﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
using Mix.Services.Databases.Lib.Enums;

namespace Mix.Services.Databases.Lib.Entities.EntityConfigurations
{
    public class MetadataContentAssociationConfiguration : EntityBaseConfiguration<MixMetadataContentAssociation, int>
    {
        public MetadataContentAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixMetadataContentAssociation> builder)
        {
            builder.ToTable(MixServicesDatabasesConstants.DatabaseNameMetadataContentAssociation);
            base.Configure(builder);
            builder.Property(e => e.ContentType)
               .IsRequired(false)
               .HasConversion(new EnumToStringConverter<MetadataParentType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}