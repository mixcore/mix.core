using Mix.Database.Entities.Account;

namespace Mix.Database.EntityConfigurations.Base.Account
{
    internal class MixUserTenantConfiguration<TConfig> : IEntityTypeConfiguration<MixUserTenant>
         where TConfig : IDatabaseConstants
    {
        protected virtual IDatabaseConstants Config { get; set; }
        public virtual void Configure(EntityTypeBuilder<MixUserTenant> builder)
        {
            Config = (TConfig)Activator.CreateInstance(typeof(TConfig));

            builder.HasKey(e => new { e.MixUserId, e.TenantId });

            builder.HasIndex(e => e.MixUserId);

            builder.HasIndex(e => e.TenantId);


            builder.Property(e => e.MixUserId)
                 .HasDefaultValueSql(Config.GenerateUUID);
        }
    }
}
