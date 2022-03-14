using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUserClaimsConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUserClaims>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUserClaims> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.MixUserId)
                .HasColumnType($"{Config.Guid}");

            builder.Property(e => e.ClaimType)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType($"{Config.Guid}");

            builder.HasOne(d => d.MixUser)
                .WithMany(p => p.AspNetUserClaimsApplicationUser)
                .HasForeignKey(d => d.MixUserId);

            builder.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserClaimsUser)
                .HasForeignKey(d => d.UserId);
        }
    }
}
