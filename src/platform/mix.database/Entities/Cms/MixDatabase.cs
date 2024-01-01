using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabase : EntityBase<int>
    {
        public int? MixDatabaseContextId { get; set; }
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public MixDatabaseType Type { get; set; }
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
        [NotMapped]
        public virtual ICollection<MixDatabaseRelationship> SourceRelationships { get; set; }
        [NotMapped]
        public virtual ICollection<MixDatabaseRelationship> DestinateRelationships { get; set; }
    }
}
