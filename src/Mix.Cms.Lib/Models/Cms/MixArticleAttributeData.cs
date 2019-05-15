using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixArticleAttributeData
    {
        public MixArticleAttributeData()
        {
            MixArticleAttributeValue = new HashSet<MixArticleAttributeValue>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Value { get; set; }
        public string Fields { get; set; }
        public int Priority { get; set; }
        public int? ProductId { get; set; }
        public int Status { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual MixArticle MixArticle { get; set; }
        public virtual ICollection<MixArticleAttributeValue> MixArticleAttributeValue { get; set; }
    }
}
