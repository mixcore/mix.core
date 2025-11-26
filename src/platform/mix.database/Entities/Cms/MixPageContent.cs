using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPageContent : ExtraColumnMultilingualSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public virtual MixPage MixPage { get; set; }
    }
}
