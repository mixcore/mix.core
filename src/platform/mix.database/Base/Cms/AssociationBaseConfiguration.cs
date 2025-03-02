using Mix.Database.EntityConfigurations;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Base.Cms
{
    public abstract class AssociationBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : AssociationBase<TPrimaryKey>

    {
        protected AssociationBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.TenantId)
                .HasColumnName("tenant_id");
            builder.Property(e => e.ParentId)
                .HasColumnName("parent_id");
            builder.Property(e => e.ChildId)
                .HasColumnName("child_id");

        }

    }
}
