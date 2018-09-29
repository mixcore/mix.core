using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPage
    {
        public MixPortalPage()
        {
            MixPortalPageNavigationIdNavigation = new HashSet<MixPortalPageNavigation>();
            MixPortalPageNavigationParent = new HashSet<MixPortalPageNavigation>();
            MixPortalPagePosition = new HashSet<MixPortalPagePosition>();
            MixPortalPageRole = new HashSet<MixPortalPageRole>();
        }

        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public string Icon { get; set; }
        public string TextKeyword { get; set; }
        public int Status { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string TextDefault { get; set; }
        public int Level { get; set; }

        public ICollection<MixPortalPageNavigation> MixPortalPageNavigationIdNavigation { get; set; }
        public ICollection<MixPortalPageNavigation> MixPortalPageNavigationParent { get; set; }
        public ICollection<MixPortalPagePosition> MixPortalPagePosition { get; set; }
        public ICollection<MixPortalPageRole> MixPortalPageRole { get; set; }
    }
}
