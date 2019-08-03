using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostAttributeData
    {
        public MixPostAttributeData()
        {
            MixArticleAttributeValue = new HashSet<MixArticleAttributeValue>();
        }

        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int AttributeSetId { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPost MixArticle { get; set; }
        public virtual ICollection<MixArticleAttributeValue> MixArticleAttributeValue { get; set; }
    }
}
