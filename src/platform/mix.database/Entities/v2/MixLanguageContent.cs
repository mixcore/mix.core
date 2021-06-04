using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixLanguageContent : MultilanguageContentaBase<int>
    {
        public string DefaultContent { get; set; }

        public int MixLanguageId { get; set; }
        public virtual MixLanguage MixLanguage { get; set; }
    }
}
