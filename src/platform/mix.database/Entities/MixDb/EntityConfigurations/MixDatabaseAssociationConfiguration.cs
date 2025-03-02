using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.MixDb.EntityConfigurations
{
    public class MixDatabaseAssociationConfiguration : EntityBaseConfiguration<MixDatabaseAssociation, Guid>

    {
        public MixDatabaseAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixDatabaseAssociation> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.GuidChildId)
              .HasColumnName("guid_child_id")
              .HasColumnType(Config.Guid);

            builder.Property(e => e.GuidParentId)
              .HasColumnName("guid_parent_id")
              .HasColumnType(Config.Guid);

            builder.Property(e => e.ParentId)
              .HasColumnName("parent_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.ChildId)
              .HasColumnName("child_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.TenantId)
              .HasColumnName("tenant_id")
              .HasColumnType(Config.Integer);

            builder.Property(e => e.ParentDatabaseName)
                .HasColumnName("parent_database_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet)
               .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.ChildDatabaseName)
                .HasColumnName("child_database_name")
               .HasColumnType($"{Config.NString}{Config.MediumLength}")
               .HasCharSet(Config.CharSet);
        }
    }
}
