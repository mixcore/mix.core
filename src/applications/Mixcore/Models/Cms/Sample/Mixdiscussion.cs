using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdiscussion
{
    public int Id { get; set; }

    public int MixTenantId { get; set; }

    public int? ParentId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; }

    public int? IntContentId { get; set; }

    public Guid? GuidContentId { get; set; }

    public string ContentType { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }
}
