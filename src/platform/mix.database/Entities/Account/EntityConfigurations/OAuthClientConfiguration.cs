using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.Services;
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
            builder.Property(e => e.ApplicationType)
               .HasConversion(new EnumToStringConverter<ApplicationType>())
               .HasColumnType($"{Config.String}{Config.SmallLength}")
               .HasCharSet(Config.CharSet);
            
            builder.Property(e => e.Name)
                .IsRequired()
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Secret)
                .IsRequired()
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.AllowedOrigins)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.AllowedProtectedResources)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());
            
            builder.Property(e => e.AllowedScopes)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MaxLength}")
                .HasConversion(
                    v => JArray.FromObject(v).ToString(Newtonsoft.Json.Formatting.None),
                    v => JArray.Parse(v ?? "[]").ToObject<List<string>>());

            builder.Property(e => e.ClientUri)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.RedirectUris)
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
