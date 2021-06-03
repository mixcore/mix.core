using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixLanguageContent : MultilanguageContentaBase<int, MixLanguage>
    {
        public string DefaultContent { get; set; }
    }
}
