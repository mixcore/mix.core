using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPostContent : ExtraColumnMultilingualSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public int? MixPostContentId { get; set; }

        public virtual MixPost MixPost { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
