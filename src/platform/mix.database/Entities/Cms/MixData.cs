using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixData : EntityBase<Guid>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }

        public virtual MixDatabase MixDatabase { get; set; }
        public virtual ICollection<MixDataContent> MixDataContents { get; set; }
    }
}
