﻿using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixPage: SiteEntityBase<int>
    {
        public virtual ICollection<MixPageContent> MixPageContents { get; set; }
    }
}