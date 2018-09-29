using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPageRole
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int PageId { get; set; }
        public int Priority { get; set; }
        public string RoleId { get; set; }
        public int Status { get; set; }

        public MixPortalPage Page { get; set; }
    }
}
