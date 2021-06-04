using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPostContent: MultilanguageSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public string Layout { get; set; }
        public string Template { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public int MixPostId { get; set; }
        public virtual MixPost MixPost { get; set; }
        public virtual ICollection<MixPage> MixPages { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
