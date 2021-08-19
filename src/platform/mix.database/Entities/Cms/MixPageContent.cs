using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPageContent: ExtraColumnMultilanguageSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public virtual MixPage MixPage { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
