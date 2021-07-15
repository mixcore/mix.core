using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixLanguageContent : MultilanguageUniqueNameContentBase<int>
    {
        public string DefaultContent { get; set; }

        public virtual MixLanguage MixLanguage { get; set; }
    }
}
