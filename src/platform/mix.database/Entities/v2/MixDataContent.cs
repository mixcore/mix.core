﻿using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDataContent : MultilanguageSEOContentBase<Guid, MixData>
    {
        public string Layout { get; set; }
        public string Template { get; set; }
        public virtual ICollection<MixDataContentValue> MixDataContentValues{ get; set; }
    }
}