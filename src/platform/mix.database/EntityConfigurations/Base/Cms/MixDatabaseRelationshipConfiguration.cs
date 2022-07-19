using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Converters;
using Newtonsoft.Json.Linq;

namespace Mix.Database.EntityConfigurations.Base.Cms
{
    public class MixDatabaseRelationshipConfiguration<TConfig> : AssociationBaseConfiguration<MixDatabaseRelationship, int, TConfig>
        where TConfig : IDatabaseConstants
    {
        public override void Configure(EntityTypeBuilder<MixDatabaseRelationship> builder)
        {
            base.Configure(builder);

            builder.HasOne<MixDatabase>(e => e.SourceDatabase)
                .WithMany(e => e.SourceRelationships)
                .HasForeignKey(e => e.LeftId);
            
            builder.HasOne<MixDatabase>(e => e.DestinateDatabase)
                .WithMany(e => e.DestinateRelationships)
                .HasForeignKey(e => e.RightId);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseRelationshipType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
