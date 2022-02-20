using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPage : TenantEntityBase<int>
    {
        public virtual ICollection<MixPageContent> MixPageContents { get; set; }
    }
}
