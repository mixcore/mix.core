using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixlanguage
{
    public int Id { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public int MixTenantId { get; set; }

    public string SystemName { get; set; }

    public virtual Mixtenant MixTenant { get; set; }

    public virtual ICollection<Mixlanguagecontent> Mixlanguagecontents { get; } = new List<Mixlanguagecontent>();
}
