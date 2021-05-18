using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixDatabaseData: AuditedEntity
    {
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixDatabase MixDatabase { get; set; }
    }
}