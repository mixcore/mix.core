using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUserRolesConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUserRoles>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUserRoles> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasKey(e => new { e.UserId, e.RoleId });

            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.UserId)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.RoleId)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.MixUserId)
               .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.HasOne(d => d.MixUser)
                .WithMany(p => p.AspNetUserRolesApplicationUser)
                .HasForeignKey(d => d.MixUserId);

            builder.HasOne(d => d.Role)
                .WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.RoleId);

            builder.HasOne(d => d.User)
                .WithMany(p => p.AspNetUserRolesUser)
                .HasForeignKey(d => d.UserId);
        }
    }
}
