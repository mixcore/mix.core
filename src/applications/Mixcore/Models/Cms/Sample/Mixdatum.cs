using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatum
{
    public Guid Id { get; set; }

    public int MixTenantId { get; set; }

    public int MixDatabaseId { get; set; }

    public string MixDatabaseName { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Mixdatabase MixDatabase { get; set; }

    public virtual ICollection<Mixdatacontent> Mixdatacontents { get; } = new List<Mixdatacontent>();
}
