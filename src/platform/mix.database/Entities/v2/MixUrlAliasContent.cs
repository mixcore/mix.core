using Mix.Database.Entities.Base;
using Mix.Shared.Enums;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixUrlAliasContent : MultilanguageContentBase<int>
    {
        public string SourceId { get; set; }
        public MixUrlAliasType Type { get; set; }
        public string Alias { get; set; }

        public int MixUrlAliasId { get; set; }
        public virtual MixUrlAlias MixUrlAlias { get; set; }
    }
}
