using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixpagemoduleassociation
{
    public int Id { get; set; }

    public int? MixPageContentId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public int Status { get; set; }

    public bool IsDeleted { get; set; }

    public int MixTenantId { get; set; }

    public int ParentId { get; set; }

    public int ChildId { get; set; }

    public virtual Mixpagecontent MixPageContent { get; set; }
}
