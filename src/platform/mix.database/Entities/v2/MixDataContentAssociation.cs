using Mix.Database.Entities.Base;
using Mix.Shared.Enums;
using System;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDataContentAssociation : MultilanguageContentBase<Guid>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public Guid DataContentId { get; set; }
        public Guid GuidParentId{ get; set; }
        public int IntParentId { get; set; }
    }
}
