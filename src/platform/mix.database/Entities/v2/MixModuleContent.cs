using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixModuleContent : MultilanguageSEOContentBase<int>
    {
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        public int MixModuleId { get; set; }
        public virtual MixModule MixModule { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
