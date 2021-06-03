using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPost: EntityBase<int>
    {
        public ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
