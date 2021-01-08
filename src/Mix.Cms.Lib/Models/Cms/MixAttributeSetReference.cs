﻿using System;
using System.Collections.Generic;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixAttributeSetReference
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int ParentType { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int AttributeSetId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }

        public virtual MixAttributeSet AttributeSet { get; set; }
    }
}
