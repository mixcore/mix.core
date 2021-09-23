using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixCulture : TenantEntityBase<int>
    {
        public string Alias { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }
    }
}
