using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Models.Account;
using System;
using System.Linq;
using System.Reflection;

namespace Mix.Cms.Lib.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyAllConfigurationsFromNamespace(
        this ModelBuilder modelBuilder, Assembly assembly, string ns)
        {
            var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Single(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
            && m.GetParameters().Count() == 1
            && m.GetParameters().Single().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
            foreach (var type in assembly.GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters
                    && c.Namespace == ns
                ))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
            return modelBuilder;
        }

        public static ModelBuilder ApplyPostgresIddentityConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClaimType).HasMaxLength(400);

                entity.Property(e => e.ClaimValue).HasMaxLength(400);

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasDatabaseName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id)
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8")
                    .UseCollation("und-x-icu");

                entity.Property(e => e.ConcurrencyStamp)
                .HasColumnType("varchar(400)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

                entity.Property(e => e.Name).HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");

                entity.Property(e => e.NormalizedName).HasColumnType("varchar(250)")
                .HasCharSet("utf8")
                .UseCollation("und-x-icu");
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.Property(e => e.ClaimType).HasMaxLength(400);

                entity.Property(e => e.ClaimValue).HasMaxLength(400);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserClaimsApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaimsUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_1");

                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(50);

                entity.Property(e => e.ProviderKey).HasMaxLength(50);

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.Property(e => e.ProviderDisplayName).HasMaxLength(400);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserLoginsApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLoginsUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasMaxLength(50);

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserRolesApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRolesUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.LoginProvider).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(400);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasDatabaseName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(250);

                entity.Property(e => e.Culture).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("timestamp without time zone");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.JoinDate).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastModified).HasColumnType("timestamp without time zone");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LockoutEnd).HasColumnType("timestamp without time zone");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(250);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(250);

                entity.Property(e => e.PasswordHash).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RegisterType).HasMaxLength(50);

                entity.Property(e => e.SecurityStamp).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(250);
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AllowedOrigin).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ClientId).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ExpiresUtc).HasColumnType("timestamp without time zone");

                entity.Property(e => e.IssuedUtc).HasColumnType("timestamp without time zone");

                entity.Property(e => e.Username).HasMaxLength(250);
            });
            return modelBuilder;
        }

        public static ModelBuilder ApplyIddentityConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ClaimType).HasMaxLength(400);

                entity.Property(e => e.ClaimValue).HasMaxLength(400);

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasDatabaseName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(400);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.NormalizedName).HasMaxLength(250);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.Property(e => e.ClaimType).HasMaxLength(400);

                entity.Property(e => e.ClaimValue).HasMaxLength(400);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserClaimsApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaimsUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins_1");

                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(50);

                entity.Property(e => e.ProviderKey).HasMaxLength(50);

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.Property(e => e.ProviderDisplayName).HasMaxLength(400);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserLoginsApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLoginsUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.ApplicationUserId);

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.RoleId).HasMaxLength(50);

                entity.Property(e => e.ApplicationUserId).HasMaxLength(50);

                entity.HasOne(d => d.ApplicationUser)
                    .WithMany(p => p.AspNetUserRolesApplicationUser)
                    .HasForeignKey(d => d.ApplicationUserId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRolesUser)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.LoginProvider).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(400);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasDatabaseName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.ConcurrencyStamp).HasMaxLength(250);

                entity.Property(e => e.Culture).HasMaxLength(50);

                entity.Property(e => e.Dob)
                    .HasColumnName("DOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.Gender).HasMaxLength(50);

                entity.Property(e => e.JoinDate).HasColumnType("datetime");

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.LockoutEnd).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(250);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(250);

                entity.Property(e => e.PasswordHash).HasMaxLength(250);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.RegisterType).HasMaxLength(50);

                entity.Property(e => e.SecurityStamp).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(250);
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.AllowedOrigin).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Secret)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RefreshTokens>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.ClientId).HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ExpiresUtc).HasColumnType("datetime");

                entity.Property(e => e.IssuedUtc).HasColumnType("datetime");

                entity.Property(e => e.Username).HasMaxLength(250);
            });
            return modelBuilder;
        }
    }
}