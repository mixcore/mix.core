using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Shared.Enums;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    public class OAuthClientConfiguration : EntityBaseConfiguration<OAuthClient, Guid>
    {
        public OAuthClientConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public override void Configure(EntityTypeBuilder<OAuthClient> builder)
        {
            builder.ToTable("o_auth_client");

            builder.Property(e => e.RefreshTokenLifeTime)
                .HasColumnName("refresh_token_life_time");
            
            builder.Property(e => e.UsePkce)
                .HasColumnName("use_pkce")
                 .HasColumnType(Config.Boolean);

            builder.Property(e => e.IsActive)
                .HasColumnName("is_active")
                 .HasColumnType(Config.Boolean);
            
            builder.Property(e => e.GrantTypes)
                .HasColumnName("grant_types")
                 .HasColumnType(Config.Text)
                 .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>()); ;

            builder.Property(e => e.ApplicationType)
                .HasColumnName("application_type")
               .HasConversion(new EnumToStringConverter<ApplicationType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Secret)
                .IsRequired()
                .HasColumnName("secret")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.AllowedOrigins)
                .HasColumnName("allowed_origins")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.AllowedProtectedResources)
                .HasColumnName("allowed_protected_resources")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());
            
            builder.Property(e => e.AllowedScopes)
                .HasColumnName("allowed_scopes")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.ClientUri)
                .HasColumnName("client_uri")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.RedirectUris)
                .HasColumnName("redirect_uris")
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => $"[{string.Join('.', v.ToArray())}]",
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            base.Configure(builder);
        }
    }
}
