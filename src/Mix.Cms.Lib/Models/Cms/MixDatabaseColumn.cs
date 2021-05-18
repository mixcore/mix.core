using Mix.Cms.Lib.Enums;
using Mix.Heart.Infrastructure.Entities;
using System;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixDatabaseColumn: AuditedEntity
    {
        public int Id { get; set; }
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public string Configurations { get; set; }
        public string Regex { get; set; }
        public string Title { get; set; }
        public MixDataType DataType { get; set; }
        public string DefaultValue { get; set; }
        public string Name { get; set; }
        public string Options { get; set; }
        public bool IsRequire { get; set; }
        public bool IsEncrypt { get; set; }
        public bool IsMultiple { get; set; }
        public bool IsSelect { get; set; }
        public bool IsUnique { get; set; }
        public int? ReferenceId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public int Priority { get; set; }
        public MixContentStatus Status { get; set; }
    }
}