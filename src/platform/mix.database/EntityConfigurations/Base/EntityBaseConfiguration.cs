using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mix.Database.EntityConfigurations.Base
{
    public abstract class EntityBaseConfiguration<T, TPrimaryKey, TConfig> : IEntityTypeConfiguration<T>
        where TPrimaryKey : IComparable
        where T : EntityBase<TPrimaryKey>
        where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));
            string key = $"PK_{typeof(T).Name}";
            builder.HasKey(e => new { e.Id })
                   .HasName(key);

            builder.Property(e => e.Id)
                .UseDefaultGUIDIf(typeof(TPrimaryKey) == typeof(Guid), Config.GenerateUUID)
                .UseIncreaseValueIf(typeof(TPrimaryKey) == typeof(int));


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
