using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;
using System;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class AspNetUsersConfiguration<TConfig> : IEntityTypeConfiguration<AspNetUsers>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<AspNetUsers> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");

            builder.HasIndex(e => e.NormalizedUserName)
                .HasDatabaseName("UserNameIndex")
                .IsUnique()
                .HasFilter("(NormalizedUserName IS NOT NULL)");

            builder.Property(e => e.Id)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Avatar)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Culture)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Dob)
                .HasColumnName("DOB")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Email)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.FirstName)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Gender)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.JoinDate)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastName)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.LockoutEnd)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ModifiedBy)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NickName)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.NormalizedEmail)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NormalizedUserName)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PasswordHash)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PhoneNumber)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.RegisterType)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.SecurityStamp)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.UserName)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
