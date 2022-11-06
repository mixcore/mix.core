using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDataContent : MultilingualSEOContentBase<Guid>
    {
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public MixData MixData { get; set; }
        public virtual ICollection<MixDataContentValue> MixDataContentValues { get; set; }
    }
}
