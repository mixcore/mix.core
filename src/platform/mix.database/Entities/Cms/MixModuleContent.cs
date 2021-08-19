using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixModuleContent : ExtraColumnMultilanguageSEOContentBase<int>
    {
        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
