using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSet
    {
        public MixAttributeSet()
        {
            MixArticle = new HashSet<MixArticle>();
            MixAttributeField = new HashSet<MixAttributeField>();
            MixModule = new HashSet<MixModule>();
            MixPage = new HashSet<MixPage>();
        }

        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Fields { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public DateTime? UpdatedDateTime { get; set; }

        public virtual ICollection<MixArticle> MixArticle { get; set; }
        public virtual ICollection<MixAttributeField> MixAttributeField { get; set; }
        public virtual ICollection<MixModule> MixModule { get; set; }
        public virtual ICollection<MixPage> MixPage { get; set; }
    }
}
