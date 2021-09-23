using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixLanguage : TenantEntityUniqueNameBase<int>
    {
        public virtual ICollection<MixLanguageContent> MixLanguageContents { get; set; }
    }
}
