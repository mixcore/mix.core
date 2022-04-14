using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.POSTGRES.Base;


namespace Mix.Database.EntityConfigurations.POSTGRES
{
    public class MixModuleConfiguration : PostgresTenantEntityUniqueNameBaseConfiguration<MixModule, int>
    {
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
