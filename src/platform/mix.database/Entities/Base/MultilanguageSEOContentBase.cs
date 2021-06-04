using Mix.Database.Entities.Cms.v2;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageSEOContentBase<TPrimaryKey>: MultilanguageContentBase<TPrimaryKey>
    {
        public string Layout { get; set; }
        public string Template { get; set; }
        public string Image { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public DateTime? PublishedDateTime { get; set; }
        public string MixDatabaseName { get; set; }

        public Guid MixDataContentId { get; set; }
        public MixDataContent MixDataContent { get; set; }
    }
}
