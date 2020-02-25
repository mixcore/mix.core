using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixRelatedAttributeSet
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int ParentId { get; set; }
        public int ParentType { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public virtual MixAttributeSet IdNavigation { get; set; }
    }
}