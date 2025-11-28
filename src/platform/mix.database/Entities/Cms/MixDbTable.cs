using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Cms
{
    public class MixDbTable : TenantEntityBase<int>
    {
        public int? MixDbDatabaseId { get; set; }
        public string SystemName { get; set; }
        public MixDbTableType Type { get; set; }
        public List<string> ReadPermissions { get; set; }
        public List<string> CreatePermissions { get; set; }
        public List<string> UpdatePermissions { get; set; }
        public List<string> DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public virtual ICollection<MixDbColumn> MixDbColumns { get; set; }
        [NotMapped]
        public virtual ICollection<MixDbTableRelationship> SourceRelationships { get; set; }
        [NotMapped]
        public virtual ICollection<MixDbTableRelationship> DestinateRelationships { get; set; }
    }
}
