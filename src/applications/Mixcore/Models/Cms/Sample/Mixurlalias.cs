using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixurlalias
{
    public int Id { get; set; }

    public int? SourceContentId { get; set; }

    public Guid? SourceContentGuidId { get; set; }

    public string Alias { get; set; }

    public int Type { get; set; }

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

    public virtual Mixtenant MixTenant { get; set; }
}
