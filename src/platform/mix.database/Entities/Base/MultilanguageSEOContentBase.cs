using System;

namespace Mix.Database.Entities.Base
{
    public abstract class MultilanguageSEOContentBase<TPrimaryKey, TParentType>: EntityBase<TPrimaryKey>, IExternalDataEntity
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
        public Guid ExternalDataId { get; set; }
        public string ExternalDatbaseName { get; set; }
        public DateTime? PublishedDateTime { get; set; }

        public TParentType Parent { get; set; }
    }
}
