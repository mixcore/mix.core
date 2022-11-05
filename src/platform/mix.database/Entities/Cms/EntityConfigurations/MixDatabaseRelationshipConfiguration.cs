using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Converters;
using Mix.Database.Services;
using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDatabaseRelationshipConfiguration : EntityBaseConfiguration<MixDatabaseRelationship, int>
        
    {
        public MixDatabaseRelationshipConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseRelationship> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.SourceDatabaseName)
               .IsRequired()
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.DisplayName)
               .IsRequired()
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.DestinateDatabaseName)
               .IsRequired()
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.HasOne(e => e.SourceDatabase)
                .WithMany(e => e.SourceRelationships)
                .HasForeignKey(e => e.ParentId);
            
            builder.HasOne(e => e.DestinateDatabase)
                .WithMany(e => e.DestinateRelationships)
                .HasForeignKey(e => e.ChildId);

            builder.Property(e => e.Type)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixDatabaseRelationshipType>())
               .HasColumnType($"{Config.NString}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
