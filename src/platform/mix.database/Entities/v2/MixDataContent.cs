using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixDataContent : MultilanguageSEOContentBase<Guid>
    {
        public MixData MixData { get; set; }
        public virtual ICollection<MixDataContentValue> MixDataContentValues{ get; set; }
    }
}
