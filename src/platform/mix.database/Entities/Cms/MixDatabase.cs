using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabase : EntityBase<int>
    {
        public string SystemName { get; set; }
        public string DisplayName { get; set; }
        public virtual string Description { get; set; }
        public MixDatabaseType Type { get; set; }
        public string ReadPermissions { get; set; }
        public string CreatePermissions { get; set; }
        public string UpdatePermissions { get; set; }
        public string DeletePermissions { get; set; }
        public bool SelfManaged { get; set; }

        public virtual ICollection<MixDatabaseColumn> MixDatabaseColumns { get; set; }
        [NotMapped]
        public virtual ICollection<MixDatabaseRelationship> SourceRelationships { get; set; }
        [NotMapped]
        public virtual ICollection<MixDatabaseRelationship> DestinateRelationships { get; set; }
    }
}
