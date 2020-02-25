using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixRelatedData
    {
        public string Id { get; set; }
        public int Type { get; set; }
        public string Specificulture { get; set; }
        public string ParentId { get; set; }
        public int ParentType { get; set; }
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }
}