using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixdatacontentvalue
{
    public Guid Id { get; set; }

    public string MixDatabaseColumnName { get; set; }

    public string MixDatabaseName { get; set; }

    public string DataType { get; set; }

    public bool? BooleanValue { get; set; }

    public DateTime? DateTimeValue { get; set; }

    public double? DoubleValue { get; set; }

    public int? IntegerValue { get; set; }

    public string StringValue { get; set; }

    public string EncryptValue { get; set; }

    public string EncryptKey { get; set; }

    public string EncryptType { get; set; }

    public int MixDatabaseColumnId { get; set; }

    public int MixDatabaseId { get; set; }

    public Guid? MixDataContentId { get; set; }

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

    public virtual Mixdatacontent MixDataContent { get; set; }

    public virtual Mixdatabasecolumn MixDatabaseColumn { get; set; }
}
