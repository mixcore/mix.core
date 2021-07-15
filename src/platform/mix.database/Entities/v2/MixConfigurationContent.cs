using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixConfigurationContent : MultilanguageSEOContentBase<int>
    {
        public string DefaultContent { get; set; }

        public virtual MixConfiguration MixConfiguration { get; set; }
    }
}
