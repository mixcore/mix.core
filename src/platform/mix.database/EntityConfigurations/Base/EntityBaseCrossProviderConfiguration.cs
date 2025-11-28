using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Services.MixGlobalSettings;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey>(DatabaseService databaseService) : SimpleEntityBaseConfiguration<T, TPrimaryKey>(databaseService)
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CreatedDateTime)
                .HasColumnName("created_date_time")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.LastModified)
                .HasColumnName("last_modified")
                .HasColumnType(Config.DateTime);

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by")
                .HasColumnType($"{Config.String}{Config.MediumLength}");

            builder.Property(e => e.Priority)
                .HasColumnName("priority")
                .HasColumnType(Config.Integer);
            
            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted")
                .HasColumnType(Config.Boolean);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasConversion(new EnumToStringConverter<MixContentStatus>())
                .HasColumnType($"{Config.String}{Config.SmallLength}")
                .HasCharSet(Config.CharSet);
        }
    }
}
