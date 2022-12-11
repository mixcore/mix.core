using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatabasecolumn
{
    public int Id { get; set; }

    public string SystemName { get; set; }

    public string DisplayName { get; set; }

    public string MixDatabaseName { get; set; }

    public string DataType { get; set; }

    public string Configurations { get; set; }

    public int? ReferenceId { get; set; }

    public string DefaultValue { get; set; }

    public int MixDatabaseId { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastModified { get; set; }

    public string CreatedBy { get; set; }

    public string ModifiedBy { get; set; }

    public int Priority { get; set; }

    public string Status { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Mixdatabase MixDatabase { get; set; }

    public virtual ICollection<Mixdatacontentvalue> Mixdatacontentvalues { get; } = new List<Mixdatacontentvalue>();
}
