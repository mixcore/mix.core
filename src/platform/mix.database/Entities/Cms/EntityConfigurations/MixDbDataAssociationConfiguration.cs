using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Cms.EntityConfigurations
{
    public class MixDbDataAssociationConfiguration : EntityBaseConfiguration<MixDbDataAssociation, int>

    {
        public MixDbDataAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDbDataAssociation> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.ParentId)
               .HasColumnName("parent_id");

            builder.Property(e => e.ChildId)
               .HasColumnName("child_id");

            builder.Property(e => e.GuidParentId)
               .HasColumnName("guid_parent_id");

            builder.Property(e => e.GuidChildId)
               .HasColumnName("guid_child_id");

            builder.Property(e => e.ParentDatabaseName)
               .IsRequired()
               .HasColumnName("parent_database_name")
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);

            builder.Property(e => e.ChildDatabaseName)
               .IsRequired()
               .HasColumnName("child_database_name")
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);


        }
    }
}
