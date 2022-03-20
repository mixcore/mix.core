using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetRoleClaimsConfiguration<TConfig> : IEntityTypeConfiguration<AspNetRoleClaims>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetRoleClaims> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.ClaimType)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
