using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixTheme : SiteEntityBase<int>
    {
        public string PreviewUrl { get; set; }

        public string MixDatabaseName { get; set; }
        public Guid MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
        public virtual ICollection<MixViewTemplate> MixViewTemplates { get; set; }
    }
}
