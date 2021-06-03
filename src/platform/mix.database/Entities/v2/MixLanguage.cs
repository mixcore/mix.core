using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixLanguage : EntityBase<int>
    {
        public string SystemName { get; set; }
        public ICollection<MixLanguageContent> MixLanguageContents { get; set; }
    }
}
