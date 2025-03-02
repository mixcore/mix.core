using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class MixUsersConfiguration : AccountEntityBaseConfiguration<MixUser>

    {
        public MixUsersConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<MixUser> builder)
        {
            builder.ToTable("mix_users");

            var filterSql = DatabaseService.DatabaseProvider is MixDatabaseProvider.PostgreSQL
                ? "(\"normalized_user_name\" IS NOT NULL)" // PostgreSQL syntax
                : "(normalized_user_name IS NOT NULL)";
            
            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.EmailConfirmed)
                .HasColumnName("email_confirmed")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.TwoFactorEnabled)
                .HasColumnName("two_factor_enabled")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.LockoutEnabled)
                .HasColumnName("lockout_enabled")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.PhoneNumberConfirmed)
                .HasColumnName("phone_number_confirmed")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.AccessFailedCount)
                .HasColumnName("access_failed_count")
                 .HasColumnType(Config.Integer);

            builder.HasIndex(e => e.NormalizedEmail)
                    .HasDatabaseName("email_index");

            builder.HasIndex(e => e.NormalizedUserName)
                .HasDatabaseName("user_name_index")
                .IsUnique()
                .HasFilter(filterSql);

            builder.Property(e => e.Id)
                .HasColumnName("id")
                 .HasDefaultValueSql(Config.GenerateUUID);

            builder.Property(e => e.ConcurrencyStamp)
                .HasColumnName("concurrency_stamp")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Email)
                .HasColumnName("email")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.CreatedDateTime)
                .HasColumnName("created_date_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnName("last_modified")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LockoutEnd)
                .HasColumnName("lockout_end")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NormalizedEmail)
                .HasColumnName("normalized_email")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.NormalizedUserName)
                .HasColumnName("normalized_user_name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.RegisterType)
                .HasColumnName("register_type")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.SecurityStamp)
                .HasColumnName("security_stamp")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.UserName)
                .HasColumnName("user_name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

        }
    }
}
