using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatacontentassociation
{
    public Guid Id { get; set; }

    public int MixDatabaseId { get; set; }

    public string MixDatabaseName { get; set; }

    public string ParentType { get; set; }

    public Guid DataContentId { get; set; }

    public Guid? GuidParentId { get; set; }

    public int? IntParentId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public int MixTenantId { get; set; }

    public string Specificulture { get; set; }

    public string Icon { get; set; }

    public bool IsPublic { get; set; }

    public Guid ParentId { get; set; }

    public int MixCultureId { get; set; }

    public virtual Mixculture MixCulture { get; set; }
}
