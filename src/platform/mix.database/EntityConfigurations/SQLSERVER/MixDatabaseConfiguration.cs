using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;


namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixDatabaseConfiguration : SqlServerTenantEntityUniqueNameBaseConfiguration<MixDatabase, int>
    {
        public override void Configure(EntityTypeBuilder<MixDatabase> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
