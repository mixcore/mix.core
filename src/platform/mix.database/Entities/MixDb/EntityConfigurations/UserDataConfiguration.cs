using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class UserDataConfiguration : EntityBaseConfiguration<MixUserData, int>
    {
        public UserDataConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }
        public override void Configure(EntityTypeBuilder<MixUserData> builder)
        {
            builder.ToTable(MixDbDatabaseNames.UserData);
            base.Configure(builder);

            builder.Property(e => e.Avatar)
                .IsRequired(false)
                .HasColumnName("avatar");
            builder.Property(e => e.PhoneNumber)
                .IsRequired(false)
                .HasColumnName("phone_number");
            builder.Property(e => e.Fullname)
                .IsRequired(false)
                .HasColumnName("fullname");
            builder.Property(e => e.DateOfBirth)
                .IsRequired(false)
                .HasColumnName("date_of_birth");
            builder.Property(e => e.Email)
                .IsRequired(false)
                .HasColumnName("email");
            builder.Property(e => e.ParentId)
                .IsRequired()
              .HasColumnName("parent_id");
            builder.Property(e => e.ParentType)
               .IsRequired()
               .HasColumnName("parent_type")
               .HasConversion(new EnumToStringConverter<MixDatabaseParentType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
