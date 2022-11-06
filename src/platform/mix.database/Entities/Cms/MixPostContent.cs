using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPostContent : ExtraColumnMultilingualSEOContentBase<int>
    {
        public string ClassName { get; set; }

        public virtual MixPost MixPost { get; set; }
        public virtual ICollection<MixPage> MixPages { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
