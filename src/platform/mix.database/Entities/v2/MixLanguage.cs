using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixLanguage : SiteEntityBase<int>
    {
        public virtual ICollection<MixLanguageContent> MixLanguageContents { get; set; }
    }
}
