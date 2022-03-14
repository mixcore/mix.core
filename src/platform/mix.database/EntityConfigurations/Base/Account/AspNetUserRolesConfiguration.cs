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

            builder.HasIndex(e => e.RoleId);

            builder.Property(e => e.UserId)
                .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.RoleId)
                .HasDefaultValueSql(Config.GenerateUUID);

        }
    }
}
