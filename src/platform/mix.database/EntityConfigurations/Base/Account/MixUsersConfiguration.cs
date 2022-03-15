using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class MixUsersConfiguration<TConfig> : IEntityTypeConfiguration<MixUser>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<MixUser> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            builder.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");

            builder.HasIndex(e => e.NormalizedUserName)
                .HasDatabaseName("UserNameIndex")
                .IsUnique()
                .HasFilter("(NormalizedUserName IS NOT NULL)");

            builder.Property(e => e.Id)
                 .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.Avatar)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ConcurrencyStamp)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Culture)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.DOB)
                .HasColumnName("DOB")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.Email)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.FirstName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.Gender)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.JoinDate)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.LockoutEnd)
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ModifiedBy)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NickName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.NormalizedEmail)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NormalizedUserName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PasswordHash)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PhoneNumber)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.RegisterType)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.SecurityStamp)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.UserName)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
