using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;


namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixModuleConfiguration : SqlServerTenantEntityUniqueNameBaseConfiguration<MixModule, int>
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
