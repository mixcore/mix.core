using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixModuleContent : ExtraColumnMultilingualSEOContentBase<int>
    {
        public string SystemName { get; set; }
        public string ClassName { get; set; }
        public int? PageSize { get; set; }
        public MixModuleType Type { get; set; }
        public string SimpleDataColumns { get; set; }

        public virtual MixModule MixModule { get; set; }
        public virtual ICollection<MixModulePostAssociation> MixPostContents { get; set; }
    }
}
