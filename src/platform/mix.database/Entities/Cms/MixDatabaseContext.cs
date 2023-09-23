namespace Mix.Database.Entities.Cms
{
    public class MixDatabaseContext : TenantEntityUniqueNameBase<int>
    {
        public MixDatabaseProvider DatabaseProvider { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
    }
}
