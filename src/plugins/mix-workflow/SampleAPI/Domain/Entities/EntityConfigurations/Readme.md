Contain Database Configuration Files
```
public class SampleEntityConfiguration : EntityBaseConfiguration<OnepayTransactionRequest, int>
{
    public SampleEntityConfiguration(DatabaseService databaseService) : base(databaseService)
    {
    }
    public override void Configure(EntityTypeBuilder<SampleEntityConfiguration> builder)
    {
        builder.ToTable(SamplePluginConstants.SampleEntityTableName);
    }
}
```