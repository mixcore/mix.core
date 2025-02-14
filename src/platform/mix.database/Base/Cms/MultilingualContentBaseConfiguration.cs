using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Base.Cms
{
    public abstract class MultilingualContentBaseConfiguration<T, TPrimaryKey> : EntityBaseConfiguration<T, TPrimaryKey>
        where TPrimaryKey : IComparable
        where T : MultilingualContentBase<TPrimaryKey>

    {
        protected MultilingualContentBaseConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.IsPublic)
               .HasColumnName("is_public")
               .HasColumnType(Config.Boolean);

            builder.Property(e => e.TenantId)
               .HasColumnName("tenant_id")
               .HasColumnType(Config.Integer);

            builder.Property(e => e.MixCultureId)
               .HasColumnName("mix_culture_id")
               .HasColumnType(Config.Integer);

            builder.Property(e => e.ParentId)
               .HasColumnName("parent_id");

            builder.Property(e => e.Specificulture)
                .IsRequired()
                .HasColumnName("specificulture")
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Icon)
                .IsRequired(false)
                .HasColumnName("icon")
                .HasColumnType($"{Config.NString}{Config.SmallLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }

    }
}
