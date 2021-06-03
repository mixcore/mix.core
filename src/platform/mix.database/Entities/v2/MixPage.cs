using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPage: EntityBase<int>
    {
        public ICollection<MixPageContent> MixPageContents { get; set; }
    }
}
