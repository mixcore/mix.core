using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPageNavigation
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public MixPortalPage IdNavigation { get; set; }
        public MixPortalPage Parent { get; set; }
    }
}
