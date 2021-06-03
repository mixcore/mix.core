using Mix.Database.Entities.Base;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixUrlAliasContent : MultilanguageContentaBase<int, MixUrlAlias>
    {
        public string SourceId { get; set; }
        public int Type { get; set; }
        public string Alias { get; set; }
    }
}
