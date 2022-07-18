using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixModuleConfiguration<TConfig> : TenantEntityUniqueNameBaseConfiguration<MixModule, int, TConfig>
        where TConfig : IDatabaseConstants
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
