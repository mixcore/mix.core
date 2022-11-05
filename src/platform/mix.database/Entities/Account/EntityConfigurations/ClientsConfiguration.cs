using Mix.Database.Entities.Account;
using Mix.Database.Services;

namespace Mix.Database.Entities.Account.EntityConfigurations
{
    internal class ClientsConfiguration : AccountEntityBaseConfiguration<Clients>
         
    {
        public ClientsConfiguration(DatabaseService databaseService) : base(databaseService)
        {
        }

        public virtual void Configure(EntityTypeBuilder<Clients> builder)
        {
            builder.Property(e => e.Id)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.SmallLength}");

            builder.Property(e => e.AllowedOrigin)
                .HasCharSet(Config.CharSet)
                .UseCollation(Config.DatabaseCollation)
                .HasColumnType($"{Config.String}{Config.MediumLength}");

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
        }
    }
}
