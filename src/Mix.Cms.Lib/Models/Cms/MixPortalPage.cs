using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPortalPage
    {
        public MixPortalPage()
        {
            MixPortalPageNavigationPage = new HashSet<MixPortalPageNavigation>();
            MixPortalPageNavigationParent = new HashSet<MixPortalPageNavigation>();
            MixPortalPageRole = new HashSet<MixPortalPageRole>();
        }

        public int Id { get; set; }
        public string Icon { get; set; }
        public string TextKeyword { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string TextDefault { get; set; }
        public int Level { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual ICollection<MixPortalPageNavigation> MixPortalPageNavigationPage { get; set; }
        public virtual ICollection<MixPortalPageNavigation> MixPortalPageNavigationParent { get; set; }
        public virtual ICollection<MixPortalPageRole> MixPortalPageRole { get; set; }
    }
}
