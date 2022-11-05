using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixModuleConfiguration : TenantEntityUniqueNameBaseConfiguration<MixModule, int>
        
    {
        public MixModuleConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixModule> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixModuleType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
