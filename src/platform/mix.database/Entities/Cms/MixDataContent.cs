using Mix.Database.Entities.Base;
using System;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDataContent : MultilanguageSEOContentBase<Guid>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixData MixData { get; set; }
        public virtual ICollection<MixDataContentValue> MixDataContentValues{ get; set; }
    }
}
