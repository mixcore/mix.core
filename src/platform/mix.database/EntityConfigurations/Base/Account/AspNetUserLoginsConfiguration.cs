using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUserLoginsConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUserLogins>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUserLogins> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_1");

            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.UserId);

            builder.Property(e => e.LoginProvider)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderKey)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.MixUserId)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.ProviderDisplayName)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.UserId)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.HasOne(d => d.MixUser)
                .WithMany(p => p.AspNetUserLoginsApplicationUser)
                .HasForeignKey(d => d.MixUserId);

            builder.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserLoginsUser)
                .HasForeignKey(d => d.UserId);
        }
    }
}
