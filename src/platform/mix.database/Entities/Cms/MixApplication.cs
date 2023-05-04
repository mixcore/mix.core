namespace Mix.Database.Entities.Cms
{
    public class MixApplication : TenantEntityBase<int>
    {
        public string BaseHref { get; set; }
        public string BaseRoute { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string Image { get; set; }
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }

        //public JObject? ExtraData { get; set; }
    }
}
