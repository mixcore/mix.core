Contain EF entities used in this plugins

```
public class SampleEntity : EntityBase<int>
{
}

public class SampleDbContext : BaseDbContext
{
    public SampleDbContext(DatabaseService databaseService) : base(databaseService, MixConstants.CONST_MIXDB_CONNECTION)
    {
    }

    public SampleDbContext(string connectionString, MixDatabaseProvider databaseProvider) : base(connectionString, databaseProvider)
    {
    }
    public DbSet<SampleEntity> SampleEntity { get; set; }
}
```