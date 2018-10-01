using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixRelatedArticle
    {
        public int SourceId { get; set; }
        public string Specificulture { get; set; }
        public int DestinationId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public MixArticle MixArticle { get; set; }
        public MixArticle S { get; set; }
    }
}
