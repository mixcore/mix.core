using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCategoryCategory
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Specificulture { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Priority { get; set; }
        public int Status { get; set; }

        public MixCategory MixCategory { get; set; }
        public MixCategory MixCategoryNavigation { get; set; }
    }
}
