using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixPageContent: MultilanguageSEOContentBase<int, MixPage>
    {
        public string Title { get; set; }
        public string CssClass { get; set; }
        public string Icon { get; set; }
        public string Layout { get; set; }
        public string Template { get; set; }
        public int? PageSize { get; set; }
        public MixPageType Type { get; set; }

        public ICollection<MixPostContent> MixPostContents { get; set; }
    }
}
