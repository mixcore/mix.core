using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetRolesConfiguration<TConfig> : IEntityTypeConfiguration<AspNetRoles>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetRoles> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            
            builder.HasIndex(e => e.NormalizedName)
                    .HasDatabaseName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("(NormalizedName IS NOT NULL)");

            builder.Property(e => e.Id)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ConcurrencyStamp)
            .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.Name)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
            .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);

            builder.Property(e => e.NormalizedName)
                .HasColumnType($"{Config.String}{Config.MediumLength}")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation);
        }
    }
}
