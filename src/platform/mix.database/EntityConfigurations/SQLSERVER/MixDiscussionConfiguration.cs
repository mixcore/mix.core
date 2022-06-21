using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.SQLSERVER.Base;

namespace Mix.Database.EntityConfigurations.SQLSERVER
{
    public class MixDiscussionConfiguration : SqlServerEntityBaseConfiguration<MixDiscussion, int>
    {
        public override void Configure(EntityTypeBuilder<MixDiscussion> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.Content)
              .IsRequired()
              .HasColumnType(Config.Text)
              .HasCharSet(Config.CharSet);

            builder.Property(e => e.ContentType)
             .IsRequired()
             .HasConversion(new EnumToStringConverter<MixContentType>())
             .HasColumnType($"{Config.NString}{Config.SmallLength}")
             .HasCharSet(Config.CharSet);
        }
    }
}
