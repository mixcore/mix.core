using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixModuleAttributeSet
    {
        public MixModuleAttributeSet()
        {
            MixModuleAttributeValue = new HashSet<MixModuleAttributeValue>();
        }

        public Guid Id { get; set; }
        public int ModuleId { get; set; }
        public string Specificulture { get; set; }
        public int? ArticleId { get; set; }
        public int? CategoryId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Fields { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public string Value { get; set; }

        public MixArticleModule MixArticleModule { get; set; }
        public MixModule MixModule { get; set; }
        public MixPageModule MixPageModule { get; set; }
        public ICollection<MixModuleAttributeValue> MixModuleAttributeValue { get; set; }
    }
}
