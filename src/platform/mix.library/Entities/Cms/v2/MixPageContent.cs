using Mix.Lib.Entities.Base;
using Mix.Lib.Enums;

namespace Mix.Lib.Entities.Cms.v2
{
    public class MixPageContent: MultilanguageEntityBase<int>
    {
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Layout { get; set; }
        public string Template { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }
    }
}
