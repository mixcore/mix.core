using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPagePosition
    {
        public int PositionId { get; set; }
        public int PortalPageId { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public MixPortalPage PortalPage { get; set; }
        public MixPosition Position { get; set; }
    }
}
