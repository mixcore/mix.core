using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixDomain : TenantEntityBase<int>
    {
        public string Host { get; set; }
    }
}
