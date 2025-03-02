using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    public class SysMixDatabaseAssociationConfiguration : EntityBaseConfiguration<SysMixDatabaseAssociation, int>

    {
        public SysMixDatabaseAssociationConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<SysMixDatabaseAssociation> builder)
        {
            builder.ToTable(MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP);
            base.Configure(builder);
            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id");
            builder.Property(e => e.GuidParentId)
                .HasColumnName("guid_parent_id")
                .HasColumnType(Config.Guid); 
            builder.Property(e => e.GuidChildId)
                .HasColumnName("guid_child_id")
                .HasColumnType(Config.Guid);
            builder.Property(e => e.ParentId)
                .HasColumnName("parent_id");
            builder.Property(e => e.ChildId)
                .HasColumnName("child_id");
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
