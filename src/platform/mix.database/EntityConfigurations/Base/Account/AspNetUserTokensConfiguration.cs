using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

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
                .HasColumnType($"{Config.Guid}");

            builder.Property(e => e.LoginProvider)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Name)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Value)
                .HasColumnType($"{Config.String}{Config.MaxLength}");

            builder.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserTokens)
                .HasForeignKey(d => d.UserId);
        }
    }
}
