using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixContributorConfiguration : SqlServerEntityBaseConfiguration<MixContributor, int>
    {
        public override void Configure(EntityTypeBuilder<MixContributor> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.ContentType)
             .IsRequired()
             .HasConversion(new EnumToStringConverter<MixContentType>())
             .HasColumnType($"{Config.NString}{Config.SmallLength}")
             .HasCharSet(Config.CharSet);
        }
    }
}
