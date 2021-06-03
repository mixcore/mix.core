using Mix.Lib.Entities.Base;
using System.Collections.Generic;

namespace Mix.Lib.Entities.Cms.v2
{
    public class MixPage: EntityBase<int>
    {
        public ICollection<MixPageContent> MixPageContents { get; set; }
    }
}
