using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

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

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.ClaimType)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ClaimValue)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.RoleId)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.HasOne(d => d.Role)
                .WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId);
        }
    }
}
