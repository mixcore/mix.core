
using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixDatabaseContentAssociation
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string DataId { get; set; }
        public string ParentId { get; set; }
        public MixDatabaseContentAssociationType ParentType { get; set; }
        public int AttributeSetId { get; set; }
        public string AttributeSetName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}
