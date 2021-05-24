using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixConfiguration: AuditedEntity
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Keyword { get; set; }
        public string Category { get; set; }
        public MixDataType DataType { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixCulture SpecificultureNavigation { get; set; }
    }
}