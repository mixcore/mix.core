using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Metadata
{
    public int Id { get; set; }

    public DateTime? CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public int? MixTenantId { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool? IsDeleted { get; set; }

    public string SeoContent { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string Type { get; set; }
}
