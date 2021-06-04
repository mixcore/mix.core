using Mix.Database.Entities.Cms.v2;
using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageSEOContentBase<TPrimaryKey>: EntityBase<TPrimaryKey>
    {
        public string Specificulture { get; set; }
        public string Image { get; set; }
        public string DisplayName { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string SeoDescription { get; set; }
        public string SeoKeywords { get; set; }
        public string SeoName { get; set; }
        public string SeoTitle { get; set; }
        public string ExternalDatbaseName { get; set; }
        public DateTime? PublishedDateTime { get; set; }

        public string MixDatabaseName { get; set; }
        public Guid MixDataContentId { get; set; }

        public int MixCultureId { get; set; }
        public virtual MixCulture MixCulture { get; set; }
    }
}
