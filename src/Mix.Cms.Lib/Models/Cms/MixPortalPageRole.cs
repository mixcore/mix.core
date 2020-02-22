using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPageRole
    {
        public int PageId { get; set; }
        public string RoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public virtual MixPortalPage Page { get; set; }
    }
}