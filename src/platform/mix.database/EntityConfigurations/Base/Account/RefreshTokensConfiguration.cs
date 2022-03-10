using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class RefreshTokensConfiguration<TConfig> : IEntityTypeConfiguration<RefreshTokens>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<RefreshTokens> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.Property(e => e.Id).HasMaxLength(50);

            builder.Property(e => e.ClientId)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ExpiresUtc)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.IssuedUtc)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Username)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
