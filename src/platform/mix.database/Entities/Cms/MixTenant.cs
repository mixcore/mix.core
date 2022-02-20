using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixTenant : EntityBase<int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MixPage> MixPages { get; set; }
        public virtual ICollection<MixModule> MixModules { get; set; }
        public virtual ICollection<MixPost> MixPosts { get; set; }
        public virtual ICollection<MixDatabase> MixDatabases { get; set; }
    }
}
