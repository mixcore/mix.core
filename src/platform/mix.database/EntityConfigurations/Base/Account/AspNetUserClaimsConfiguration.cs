using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUserClaimsConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUserClaims>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.Id);

            builder.Property(e => e.ClaimType)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.HasOne(d => d.MixUser)
                    .WithMany(p => p.AspNetUserClaimsUser)
                    .HasForeignKey(d => d.UserId);

        }
    }
}
