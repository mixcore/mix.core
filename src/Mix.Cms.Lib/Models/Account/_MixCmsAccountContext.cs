﻿// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT.
// See the LICENSE file in the project root for more information.

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Mix.Identity.Data;
using MySqlConnector;
using Mix.Cms.Lib.Enums;
namespace Mix.Cms.Lib.Models.Account
{
    public partial class MixCmsAccountContext : DbContext
    {
        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<RefreshTokens> RefreshTokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsAccountContext(DbContextOptions<ApplicationDbContext> options)
                    : base(options)
        {
        }

        public MixCmsAccountContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                var provider = System.Enum.Parse<MixDatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
                switch (provider)
                {
                    case MixDatabaseProvider.MSSQL:
                        optionsBuilder.UseSqlServer(cnn);
                        break;
                    case MixDatabaseProvider.MySQL:
                        optionsBuilder.UseMySql(cnn, ServerVersion.AutoDetect(cnn));
                        break;
                    default:
                        break;
                }
            }
        }
        //Ref https://github.com/dotnet/efcore/issues/10169
        public override void Dispose()
        {

            var provider = System.Enum.Parse<MixDatabaseProvider>(MixService.GetConfig<string>(MixConstants.CONST_SETTING_DATABASE_PROVIDER));
            switch (provider)
            {
                case MixDatabaseProvider.MSSQL:
                    SqlConnection.ClearPool((SqlConnection)Database.GetDbConnection());
                    break;
                case MixDatabaseProvider.MySQL:
                    MySqlConnection.ClearPool((MySqlConnection)Database.GetDbConnection());
                    break;
            }
            base.Dispose();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
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

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}