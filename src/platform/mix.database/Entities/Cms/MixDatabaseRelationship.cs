using Mix.Database.Entities.Base;

using System.Collections.Generic;

namespace Mix.Database.Entities.Cms
{
    public class MixDatabaseRelationship : AssociationBase<int>
    {
        public string SourceDatabaseName { get; set; }
        public string DestinateDatabaseName { get; set; }
        public MixDatabaseRelationshipType Type { get; set; }
        public virtual MixDatabase SourceDatabase { get; set; }
        public virtual MixDatabase DestinateDatabase { get; set; }
    }
}
