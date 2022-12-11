using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixcache
{
    public Guid Id { get; set; }

    public string Keyword { get; set; }

    public string Value { get; set; }

    public DateTime? ExpiredDateTime { get; set; }

    public string ModifiedBy { get; set; }

    public string CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }
}
