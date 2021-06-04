using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPost: SiteEntityBase<int>
    {
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
