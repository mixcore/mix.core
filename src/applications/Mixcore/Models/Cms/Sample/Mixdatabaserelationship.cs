using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatabaserelationship
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public int ChildId { get; set; }

    public string DisplayName { get; set; }

    public string SourceDatabaseName { get; set; }

    public string DestinateDatabaseName { get; set; }

    public string Type { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }
}
