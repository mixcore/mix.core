﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class UserDataConfiguration : EntityBaseConfiguration<MixUserData, int>
    {
        public UserDataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixUserData> builder)
        {
            builder.ToTable(MixDbDatabaseNames.DatabaseNameUserData);
            base.Configure(builder);

            builder.Property(e => e.Avatar)
                .IsRequired(false);

            builder.Property(e => e.ParentType)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}