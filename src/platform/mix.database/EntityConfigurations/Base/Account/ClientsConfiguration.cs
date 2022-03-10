using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class ClientsConfiguration<TConfig> : IEntityTypeConfiguration<Clients>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<Clients> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.Property(e => e.Id)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.AllowedOrigin)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Secret)
                .IsRequired()
                .HasColumnType($"{Config.String}{Config.SmallLength}");
        }
    }
}
