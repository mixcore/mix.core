using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixCulture : SiteEntityBase<int>
    {
        public string Alias { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Icon { get; set; }
        public string Lcid { get; set; }
        public string Specificulture { get; set; }
    }
}
