using Mix.Cms.Lib.Enums;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPageRole
    {
        public int Id { get; set; }
        public int PageId { get; set; }
        public string RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixPortalPage Page { get; set; }
    }
}