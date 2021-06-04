using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixConfigurationContent : MultilanguageContentBase<int>
    {
        public string DefaultContent { get; set; }

        public int MixConfigurationId { get; set; }
        public virtual MixConfiguration MixConfiguration { get; set; }
    }
}
