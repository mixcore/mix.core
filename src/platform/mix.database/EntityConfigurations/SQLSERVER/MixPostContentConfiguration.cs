using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixPostContentConfiguration : SqlServerMultilingualSEOContentBaseConfiguration<MixPostContent, int>
    {
        public override void Configure(EntityTypeBuilder<MixPostContent> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ClassName)
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
