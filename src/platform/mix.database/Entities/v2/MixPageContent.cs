using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPageContent: MultilanguageSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public int MixPageId { get; set; }
        public virtual MixPage MixPage { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
