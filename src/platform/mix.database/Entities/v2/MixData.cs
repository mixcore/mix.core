using Mix.Database.Entities.Base;
using System.Collections.Generic;

namespace Mix.Database.Entities.Cms.v2
{
    public class MixData: EntityBase<int>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }

        public virtual MixDatabase MixDatabase { get; set; }
        public virtual ICollection<MixDataContent> MixDataContents { get; set; }
    }
}
