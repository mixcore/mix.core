using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms
{
    public class MixConfigurationContent : MultiLanguageUniqueNameContentBase<int>
    {
        public string DefaultContent { get; set; }

        public virtual MixConfiguration MixConfiguration { get; set; }
    }
}
