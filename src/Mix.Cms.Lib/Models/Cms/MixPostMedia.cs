using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixPostMedia
    {
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public int MediaId { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int Position { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public int Priority { get; set; }
        public MixEnums.MixContentStatus Status { get; set; }

        public virtual MixMedia MixMedia { get; set; }
        public virtual MixPost MixPost { get; set; }
    }
}
