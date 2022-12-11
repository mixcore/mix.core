using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatabase
{
    public int Id { get; set; }

    public string SystemName { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public string ReadPermissions { get; set; }

    public string CreatePermissions { get; set; }

    public string UpdatePermissions { get; set; }

    public string DeletePermissions { get; set; }

    public bool SelfManaged { get; set; }

    public int? MixDatabaseContextDatabaseAssociationId { get; set; }

    public int? MixTenantId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Mixdatabasecontextdatabaseassociation MixDatabaseContextDatabaseAssociation { get; set; }

    public virtual Mixtenant MixTenant { get; set; }

    public virtual ICollection<Mixdatum> Mixdata { get; } = new List<Mixdatum>();

    public virtual ICollection<Mixdatabasecolumn> Mixdatabasecolumns { get; } = new List<Mixdatabasecolumn>();
}
