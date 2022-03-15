using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUserTokensConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUserTokens>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUserTokens> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            builder.Property(e => e.UserId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.LoginProvider)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Name)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Value)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}");

        }
    }
}
