using Newtonsoft.Json.Linq;

namespace Mix.Database.Entities.Cms
{
    public class MixApplication : TenantEntityBase<int>
    {
        public string BaseHref { get; set; }
        public string DeployUrl { get; set; }
        public JObject AppSettings { get; set; }
        public string Domain { get; set; }
        public string BaseApiUrl { get; set; }
        public int? TemplateId { get; set; }
        public string Image { get; set; }
        public string MixDatabaseName { get; set; }
        public int? MixDbId { get; set; }
    }
}
